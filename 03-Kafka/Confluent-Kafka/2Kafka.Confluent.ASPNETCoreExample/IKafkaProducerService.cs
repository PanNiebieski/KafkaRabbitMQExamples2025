namespace _2Kafka.Confluent.ASPNETCoreExample;

public interface IKafkaProducerService
{
    Task ProduceAsync(string key, string value);
}
