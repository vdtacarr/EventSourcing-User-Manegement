using EventSourcing.Api.Aggregate;
using EventStore.ClientAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IEventStoreConnection connection = EventStoreConnection.Create(
            connectionString: "ConnectTo=tcp://localhost:1113;DefaultUserCredentials=admin:changeit;UseSslConnection=true;TargetHost=eventstore.org;ValidateServer=false",
            connectionName: "API_Application",
            builder: ConnectionSettings.Create().KeepReconnecting()
        );

connection.ConnectAsync().GetAwaiter().GetResult();
builder.Services.AddSingleton(connection);
builder.Services.AddSingleton<AggregateRepository>();
builder.Services.AddSingleton<UserAggregate>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
