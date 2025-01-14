namespace _7RabbitMQ.ASPNETCoreExample.Producer;

public interface IRabbitMqProducer
{
    void PublishMessage(string message);
}