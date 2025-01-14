namespace _2.KafkaFlow.ASPNETCoreExample;

public interface IKafkaFlowProducerService
{
    Task ProduceAsync(string key, MyMessage message);
}