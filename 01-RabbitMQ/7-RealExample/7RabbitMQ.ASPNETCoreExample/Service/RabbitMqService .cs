using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace _7RabbitMQ.ASPNETCoreExample.Service;

public class RabbitMqService : IRabbitMqService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqService(IOptions<RabbitMqConfig> config)
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
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: config.Value.QueueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public IModel GetChannel()
    {
        return _channel;
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}