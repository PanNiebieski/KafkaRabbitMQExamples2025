using EasyNetQ;

namespace _7EasyNetMQ.ASPNETCoreExample;

public class MessagePublisher : IMessagePublisher
{
    private readonly IBus _bus;

    public MessagePublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishMessageAsync(MyMessage message)
    {
        await _bus.PubSub.PublishAsync(message);
        Console.WriteLine($"Message Published: {message.Text}");
    }
}