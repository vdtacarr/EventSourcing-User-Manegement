using EventSourcing.Api.Events;

namespace EventSourcing.Api.Aggregate
{
    public abstract class Aggregate
    {
        //Oluşan tüm event'leri tutacak koleksiyon.
        protected readonly List<IEvent> events = new();
        public List<IEvent> GetEvents => events;
        //Event'lerin tutulacağı Aggregate/Stream adı.
        public string StreamName { get; private set; }
        public void SetStreamName(string streamName)
            => StreamName = streamName;
        //Stream adının atanıp atanmadığını kontrol eden fonksiyon.
        protected bool CheckStreamName()
            => string.IsNullOrEmpty(StreamName) || string.IsNullOrWhiteSpace(StreamName);
    }
}
