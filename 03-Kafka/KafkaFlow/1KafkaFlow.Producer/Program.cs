using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafka(
    kafka => kafka
        .AddCluster(cluster =>
        {
            const string topicName = "1KafkaFlow";
            cluster
                .WithBrokers(new[] { "localhost:19092" })
                .CreateTopicIfNotExists(topicName, numberOfPartitions: 3, replicationFactor: 3)
                .AddProducer(
                    name: "1KafkaFlow.Producer",
                    producer => producer
                        .DefaultTopic(topicName)
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSerializer<JsonCoreSerializer>()));
        })
);

var app = builder.Build();

app.MapPost("/add", RequestHandler.HandleAsync);

app.Run();


// Handler
public static class RequestHandler
{
    public static async Task<IResult> HandleAsync(
        IProducerAccessor producerAccessor,
        AddTaskRequest request, CancellationToken cancellationToken)
    {
        var producer = producerAccessor.GetProducer("1KafkaFlow.Producer");

        await producer.ProduceAsync(
            null,
            request
        );

        return Results.Accepted();
    }
}