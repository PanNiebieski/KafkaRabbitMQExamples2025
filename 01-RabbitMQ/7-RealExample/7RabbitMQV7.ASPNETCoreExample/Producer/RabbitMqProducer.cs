using _7RabbitMQ.ASPNETCoreExample.Service;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace _7RabbitMQ.ASPNETCoreExample.Producer;

public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly RabbitMqConfig _config;
    private IChannel _channel;

    public RabbitMqProducer(IRabbitMqService rabbitMqService, IOptions<RabbitMqConfig> config)
    {
        _rabbitMqService = rabbitMqService;
        _config = config.Value;
    }

    public async Task PublishMessage(string message)
    {
        _channel = await _rabbitMqService.GetChannelAsync();

        // Declare the queue (idempotent)
        await _channel.QueueDeclareAsync(
            queue: _config.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        MyMessage m = new MyMessage(message);

        string jsonString = JsonSerializer.Serialize(m);

        var body = Encoding.UTF8.GetBytes(jsonString);

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: _config.QueueName,
            body: body
        );

        Console.WriteLine($"Message Published: {message}");
    }
}