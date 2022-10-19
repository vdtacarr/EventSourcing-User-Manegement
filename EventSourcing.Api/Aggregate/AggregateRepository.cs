using EventSourcing.Api.Entities;
using EventSourcing.Api.Events;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

namespace EventSourcing.Api.Aggregate
{
    public class AggregateRepository
    {
        readonly IEventStoreConnection _connection;
        public AggregateRepository(IEventStoreConnection connection)
            => _connection = connection;
        //Oluşturulan event'leri Event Store'a kaydeder.
        public async Task SaveAsync<T>(T aggregate) where T : Aggregate, new()
        {
            List<EventData> events = aggregate.GetEvents
                .Select(@event => new EventData(
                    eventId: Guid.NewGuid(),
                    type: @event.GetType().Name,
                    isJson: true,
                    data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize( 
                        value: @event,
                        inputType: @event.GetType(),
                        options: new() { WriteIndented = true }
                        )),
                    metadata: Encoding.UTF8.GetBytes(@event.GetType().FullName))/*metadata : Metadata olarak binary formatta ilgili
event'in FullName bilgisini yani namespace ile birlikte full class adını tutmaktayız. Bu bilgiyi, event'leri 'Read Data Store'da
güncelleme yaparken hangi event'in gerçekleştiğini ayırt edebilmek için kullanacağız. */
                )
                .ToList();

            if (!events.Any())
                return;

            await _connection.AppendToStreamAsync(aggregate.StreamName, ExpectedVersion.Any, events);
            aggregate.GetEvents.Clear();
        }
        public async Task<dynamic> GetEvents(string streamName)
        {
            long nextSliceStart = 0L;
            List<ResolvedEvent> events = new();
            StreamEventsSlice readEvents = null;
            do
            {
                readEvents = await _connection.ReadStreamEventsForwardAsync(
                    stream: streamName,
                    start: nextSliceStart,
                    count: 4096,
                    resolveLinkTos: true
                    );

                if (readEvents.Events.Length > 0)
                    events.AddRange(readEvents.Events);

                nextSliceStart = readEvents.NextEventNumber;
            } while (!readEvents.IsEndOfStream);
            return events.Select(@event => new
            {
                @event.Event.EventNumber,
                @event.Event.EventType,
                @event.Event.Created,
                @event.Event.EventId,
                @event.Event.EventStreamId,
                Data = JsonSerializer.Deserialize(
                    json: Encoding.UTF8.GetString(@event.Event.Data),
                    returnType: Type.GetType(Encoding.UTF8.GetString(@event.Event.Metadata)) 
                    ),
                Metadata = Encoding.UTF8.GetString(@event.Event.Metadata)
            });
        }
        public async Task<User> GetData(string streamName)
        {
            dynamic events = await GetEvents(streamName);
            User user = new();
            foreach (var @event in events)
            {
                switch (@event.Data)
                {
                    case UserCreated o:
                        user.Id = o.UserId;
                        user.Name = o.Name;
                        user.UserName = o.UserName;
                        user.Email = o.Email;
                        user.EmailApprove = o.EmailApprove;
                        break;
                    case UserNameChanged o:
                        user.Name = o.NewName;
                        break;
                    case UserEmailApproved o:
                        user.EmailApprove = true;
                        break;
                }
            }
            return user;
        }
    }
}
