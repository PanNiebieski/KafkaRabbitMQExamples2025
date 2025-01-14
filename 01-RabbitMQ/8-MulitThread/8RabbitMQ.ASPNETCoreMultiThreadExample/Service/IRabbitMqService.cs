using RabbitMQ.Client;

namespace _8RabbitMQ.ASPNETCoreMultiThreadExample.Service;

public interface IRabbitMqService
{
    List<IModel> GetChannels();
}