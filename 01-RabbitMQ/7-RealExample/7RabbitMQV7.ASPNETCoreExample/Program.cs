using _7RabbitMQ.ASPNETCoreExample;
using _7RabbitMQ.ASPNETCoreExample.Producer;
using _7RabbitMQ.ASPNETCoreExample.Service;

var builder = WebApplication.CreateBuilder(args);

// Configure RabbitMQ settings
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));

// Register RabbitMQ Service
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

builder.Services.AddSingleton<InMemoryMessageStore>();

// Register RabbitMQ Consumer Worker
builder.Services.AddHostedService<RabbitMqConsumerWorker>();

// Register RabbitMQ Producer
builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();

var app = builder.Build();

app.MapGet("/", (InMemoryMessageStore store) => $"7RabbitMQ.ASPNETCoreExample RabbitMQ Consumer Worker Running\n\n{store.Read()}");

app.MapGet("/m/{message}", (string message, IRabbitMqProducer producer) =>
{
    producer.PublishMessage(message);
    return Results.Ok($"Message '{message}' added to the queue.");
});

app.Run();