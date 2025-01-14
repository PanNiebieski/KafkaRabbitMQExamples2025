namespace _7RabbitMQ.ASPNETCoreExample;

public class RabbitMqConfig
{
    public string HostName { get; set; } = "localhost";

    public int Port { get; set; } = 5672;

    public string QueueName { get; set; } = "test-queue";

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";

    public string VHostName { get; set; } = "my_vhost";

    public bool LoginNotRequired { get; set; }
}