using _2.KafkaFlow.ASPNETCoreExample;
using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Configure Kafka settings
builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("KafkaConfig"));
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddLogging(configure => configure.AddConsole());

var server = builder.Configuration["KafkaConfig:BootstrapServers"];
var topic = builder.Configuration["KafkaConfig:Topic"];
var groupid = builder.Configuration["KafkaConfig:GroupId"];

// Register KafkaFlow and dependencies
builder.Services.AddKafkaFlowHostedService(config => config
    .AddCluster(cluster => cluster
        .WithBrokers(new[] { server })
        .CreateTopicIfNotExists(topic, numberOfPartitions: 3, replicationFactor: 3)
        .AddProducer("producer-name", producer => producer
            .DefaultTopic(topic).AddMiddlewares(middlewares =>
                            middlewares
                                .AddSerializer<MyJsonCoreSerializer>()))
                .AddConsumer(consumer =>
                    consumer
                        .Topic(topic)
                        .WithName("consumer-name")
                        .WithGroupId(groupid)
                        .WithBufferSize(100)
                        //.WithWorkersCount(3)
                        .WithWorkersCount(
                                    (context, resolver) =>
                                    {
                                        // Implement a custom logic to calculate the number of workers
                                        if (IsPeakHour(DateTime.UtcNow))
                                        {
                                            return Task.FromResult(10); // High worker count during peak hours
                                        }
                                        else
                                        {
                                            return Task.FromResult(2); // Lower worker count during off-peak hours
                                        }
                                    },
                                    TimeSpan.FromMinutes(15) // Evaluate the worker count every 15 minutes
                         )
                        .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Earliest)
                        .AddMiddlewares(middlewares => middlewares
                            .Add<CatchErrorsMiddleware>()
                            .AddDeserializer<MyJsonCoreDeserializer>()
                            .AddTypedHandlers(handlers =>
                                handlers.AddHandler<KafkaFlowMessageHandler>()
                            )
                        )
                ))
                .SubscribeGlobalEvents(observers =>
                {
                    observers.MessageProduceStarted.Subscribe(eventContext =>
                    {
                        return Task.CompletedTask;
                    });

                    observers.MessageProduceCompleted.Subscribe(eventContext =>
                    {
                        return Task.CompletedTask;
                    });

                    observers.MessageProduceError.Subscribe(eventContext =>
                    {
                        return Task.CompletedTask;
                    });

                    observers.MessageConsumeStarted.Subscribe(eventContext =>
                    {
                        return Task.CompletedTask;
                    });

                    observers.MessageProduceCompleted.Subscribe(eventContext =>
                    {
                        return Task.CompletedTask;
                    });

                    observers.MessageConsumeError.Subscribe(eventContext =>
                    {
                        return Task.CompletedTask;
                    });
                }));

// Register message store and producer service
builder.Services.AddSingleton<InMemoryMessageStore>();
builder.Services.AddScoped<IKafkaFlowProducerService, KafkaFlowProducerService>();


var app = builder.Build();

// Define endpoints
app.MapGet("/", (InMemoryMessageStore store) => $"2.KafkaFlow.ASPNETCoreExample Consumer\n\n{store.Read()}");

app.MapGet("/m/{message}", async (string message, IKafkaFlowProducerService producer) =>
{
    await producer.ProduceAsync(message, new MyMessage(message, DateTime.UtcNow));
    return Results.Ok($"Message Produced: {message}");
});

app.Run();


static bool IsPeakHour(DateTime currentTimeUtc)
{
    // Define peak hours (e.g., 8 AM to 6 PM UTC)
    var peakStartHour = 8;  // 8 AM UTC
    var peakEndHour = 18;   // 6 PM UTC

    // Check if the current time falls within the peak hours
    return currentTimeUtc.Hour >= peakStartHour && currentTimeUtc.Hour < peakEndHour;
}