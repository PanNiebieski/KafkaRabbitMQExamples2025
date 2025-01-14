namespace _8RabbitMQ.ASPNETCoreMultiThreadExample;

public interface IRabbitMqProducer
{
    void PublishMessage(string message);
}