using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Reflection;
using System.Threading.Channels;

namespace _8RabbitMQ.ASPNETCoreMultiThreadExample.Service;

public class RabbitMqService : IRabbitMqService, IDisposable
{
    private readonly IConnection _connection;
    private readonly List<IModel> _channels;

    public RabbitMqService(IOptions<RabbitMqConfig> config)
    {
        List<IModel> models = new List<IModel>();

        for (int i = 0; i < config.Value.ConsumerThreads + 1; i++)
        {
            ConnectionFactory factory = null;

            if (config.Value.LoginNotRequired)
            {
                factory = new ConnectionFactory();
            }
            else
            {
                factory = new ConnectionFactory
                {
                    HostName = config.Value.HostName,
                    UserName = config.Value.UserName,
                    Password = config.Value.Password,
                    Uri = new Uri(config.Value.HostName),
                    Port = config.Value.Port,
                    VirtualHost = config.Value.VHostName,
                };
            }

            _connection = factory.CreateConnection();


            var channel = _connection.CreateModel();

            channel.QueueDeclare(queue: config.Value.QueueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            models.Add(channel);


        }

        _channels = models;

    }

    public List<IModel> GetChannels()
    {
        return _channels;
    }

    public void Dispose()
    {
        foreach (var _channel in _channels)
        {
            _channel.Close();
        }

        _connection.Close();
    }
}