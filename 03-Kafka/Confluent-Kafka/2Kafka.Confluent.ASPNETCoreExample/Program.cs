using _2Kafka.Confluent.ASPNETCoreExample;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for Kafka
var configuration = builder.Configuration;

builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("KafkaConfig"));

builder.Services.AddSingleton<InMemoryMessageStore>();

// Register Kafka Producer Service
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

// Register Kafka Consumer Worker
builder.Services.AddHostedService<KafkaConsumerWorker>();

var app = builder.Build();

app.MapGet("/", (InMemoryMessageStore store) => $"2Kafka.Confluent.ASPNETCoreExample Kafka Consumer Worker Running\n\n{store.Read()}");

app.MapGet("/m/{message}", async (string message, IKafkaProducerService producer) =>
{
    await producer.ProduceAsync("key1", message);

    return Results.Ok($"Message Produced {message}");
});

app.Run();