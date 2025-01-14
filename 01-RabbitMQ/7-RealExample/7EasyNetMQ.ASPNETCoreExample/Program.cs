using _7EasyNetMQ.ASPNETCoreExample;
using EasyNetQ;

var builder = WebApplication.CreateBuilder(args);

// Configure RabbitMQ Connection
var connectionString = builder.Configuration.GetSection("RabbitMqConfig:ConnectionString").Value;
builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus(connectionString));

// Register Publisher Service
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

builder.Services.AddSingleton<InMemoryMessageStore>();

// Register Consumer Worker
builder.Services.AddHostedService<RabbitMqConsumerWorker>();

var app = builder.Build();

app.MapGet("/", (InMemoryMessageStore store) => $"7EasyNetMQ.ASPNETCoreExample RabbitMQ Consumer Worker Running\n\n{store.Read()}");

app.MapGet("/m/{message}", async (string message, IMessagePublisher publisher) =>
{
    var myMessage = new MyMessage { Text = message };

    await publisher.PublishMessageAsync(myMessage);

    return Results.Ok($"Message '{message}' added to the queue.");
});

app.Run();