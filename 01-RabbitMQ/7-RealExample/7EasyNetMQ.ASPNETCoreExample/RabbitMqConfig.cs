namespace _7EasyNetMQ.ASPNETCoreExample;

public class RabbitMqConfig
{
    public string ConnectionString { get; set; } = "localhost";

    public string QueueName { get; set; } = "test-queue";
}