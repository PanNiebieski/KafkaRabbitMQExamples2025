using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace _7RabbitMQ.ASPNETCoreExample.Service;

public class RabbitMqService : IRabbitMqService, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;

    private readonly string _queueName;
    private readonly ConnectionFactory _factory;

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

        _queueName = config.Value.QueueName;
        _factory = factory;
    }

    public async Task Create()
    {
        if (_connection == null)
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: _queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }
    }

    public async Task<IChannel> GetChannelAsync()
    {
        await Create();

        return _channel;
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}