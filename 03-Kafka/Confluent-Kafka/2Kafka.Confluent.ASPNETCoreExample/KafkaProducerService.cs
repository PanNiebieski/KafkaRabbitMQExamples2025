using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Text.Json;

namespace _2Kafka.Confluent.ASPNETCoreExample;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly KafkaConfig _settings;

    public KafkaProducerService(IOptions<KafkaConfig> options)
    {
        _settings = options.Value;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _settings.BootstrapServers
        };
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task ProduceAsync(string key, string messagevalue)
    {
        MyMessage m = new MyMessage(messagevalue);
        string jsonString = JsonSerializer.Serialize(m);

        var message = new Message<string, string> { Key = key, Value = jsonString };
        var result = await _producer.ProduceAsync(_settings.Topic, message);
        Console.WriteLine($"Produced message to {result.TopicPartitionOffset}");
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}