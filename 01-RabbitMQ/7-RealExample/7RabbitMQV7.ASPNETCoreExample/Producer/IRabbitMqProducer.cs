namespace _7RabbitMQ.ASPNETCoreExample.Producer;

public interface IRabbitMqProducer
{
    Task PublishMessage(string message);
}