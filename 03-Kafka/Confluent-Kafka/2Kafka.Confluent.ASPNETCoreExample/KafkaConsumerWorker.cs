using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace _2Kafka.Confluent.ASPNETCoreExample;

public class KafkaConsumerWorker : BackgroundService
{
    private readonly string _topic;
    private readonly IConsumer<string, string> _consumer;
    private InMemoryMessageStore _memoryMessage;

    public KafkaConsumerWorker(IOptions<KafkaConfig> options, InMemoryMessageStore memoryMessage)
    {
        _topic = options.Value.Topic;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _memoryMessage = memoryMessage;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        return Task.Run(() =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(stoppingToken);

                        MyMessage m = JsonSerializer.Deserialize<MyMessage>(result.Message.Value);

                        _memoryMessage.Write(m);

                        Console.WriteLine($"Consumed message '{result.Message.Value}' from topic '{result.Topic}'");
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
                    }
                }
            }
            finally
            {
                _consumer.Close();
            }
        }, stoppingToken);
    }
}