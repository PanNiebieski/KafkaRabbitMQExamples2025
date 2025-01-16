namespace _2.KafkaFlow.ASPNETCoreExample;

public class KafkaConfig
{
    public string BootstrapServers { get; set; } = "localhost:9092";

    public string Topic { get; set; } = "2Kafka.Confluent.ASPNETCoreExample";

    public string GroupId { get; set; } = "NULL";
}