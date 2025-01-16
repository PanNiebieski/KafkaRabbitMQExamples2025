using EasyNetQ;

namespace _7EasyNetMQ.ASPNETCoreExample;

public class RabbitMqConsumerWorker : BackgroundService
{
    private readonly IBus _bus;
    private InMemoryMessageStore _memoryMessage;

    public RabbitMqConsumerWorker(IBus bus, InMemoryMessageStore memoryMessage)
    {
        _bus = bus;
        _memoryMessage = memoryMessage;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = _bus.PubSub.SubscribeAsync<MyMessage>("my_subscription_id", message =>
        {
            Console.WriteLine($"Message Received: {message.Text}, Timestamp: {message.Timestamp}");

            _memoryMessage.Write(message);

            return Task.CompletedTask;
        }, stoppingToken);

        while (true)
        {
            await Task.Delay(5000, stoppingToken);
            Console.WriteLine($"RabbitMqConsumerWorker Still waiting");
        }
    }
}