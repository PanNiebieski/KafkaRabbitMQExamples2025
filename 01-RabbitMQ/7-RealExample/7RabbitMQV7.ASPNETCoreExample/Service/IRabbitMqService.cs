using RabbitMQ.Client;

namespace _7RabbitMQ.ASPNETCoreExample.Service;

public interface IRabbitMqService
{
    Task<IChannel> GetChannelAsync();

    Task Create();
}