namespace _7EasyNetMQ.ASPNETCoreExample;

public interface IMessagePublisher
{
    Task PublishMessageAsync(MyMessage message);
}