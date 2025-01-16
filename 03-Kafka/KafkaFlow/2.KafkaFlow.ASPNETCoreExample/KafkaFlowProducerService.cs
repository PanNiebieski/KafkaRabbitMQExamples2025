using KafkaFlow;
using KafkaFlow.Producers;

namespace _2.KafkaFlow.ASPNETCoreExample;

public class KafkaFlowProducerService : IKafkaFlowProducerService
{
    private readonly IMessageProducer _producer;

    public KafkaFlowProducerService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("producer-name"); // Use the same name defined in KafkaFlow configuration
    }

    public async Task ProduceAsync(string key, MyMessage message)
    {
        await _producer.ProduceAsync(key, message);
        Console.WriteLine($"Produced message with key '{key}': {message.Text}");
    }
}