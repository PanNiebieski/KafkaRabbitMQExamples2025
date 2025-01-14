using _2.KafkaFlow.ASPNETCoreExample;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

const string topicName = "2Kafka.Confluent.ASPNETCoreExample";
int unique = 5;
string groupId = "2.KafkaFlow.ASPNETCoreExample_2";
var services = new ServiceCollection();
services.AddTransient<KafkaFlowMessageHandler>();

services.AddLogging(configure => configure.AddConsole());

services.AddKafkaFlowHostedService(
    kafka => kafka
        .UseMicrosoftLog()
        .AddCluster(cluster =>
        {
            cluster
                .WithBrokers(new[] { "localhost:19092" })
                .AddConsumer(consumer =>
                    consumer
                        .Topic(topicName)
                        .WithGroupId(groupId)
                        .WithBufferSize(100)
                        .WithWorkersCount(3)
                        .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Earliest)
                        .AddMiddlewares(middlewares => middlewares
                                .AddDeserializer<JsonCoreDeserializer>()
                            .AddTypedHandlers(handlers =>
                                handlers.AddHandler<KafkaFlowMessageHandler>()
                            )
                        )
                );
        })
);

services.AddLogging(configure => configure.AddConsole());

var provider = services.BuildServiceProvider();
var bus = provider.CreateKafkaBus();

await bus.StartAsync();

Console.WriteLine("Press key to exit");
Console.ReadKey();