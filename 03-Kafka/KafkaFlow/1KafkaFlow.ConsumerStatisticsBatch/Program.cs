using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

const string topicName = "1KafkaFlow";
int unique = 6;
string groupId = "1KafkaFlow_Group_" + unique.ToString();
var services = new ServiceCollection();

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
                            .Add<CatchErrorsMiddleware>()
                            .AddDeserializer<JsonCoreDeserializer>()
                            .AddBatching(5, TimeSpan.FromSeconds(5))
                            .Add<StatisticsMiddleware>()
                            .Add<ReadMiddleware>()
                        )
                );
        })
);

var provider = services.BuildServiceProvider();

var bus = provider.CreateKafkaBus();
await bus.StartAsync();

Console.WriteLine("Press key to exit");
Console.ReadKey();
