using _8RabbitMQ.ASPNETCoreMultiThreadExample.Service;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace _8RabbitMQ.ASPNETCoreMultiThreadExample.Producer;

public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly RabbitMqConfig _config;
    private readonly List<IModel> _channels;

    public RabbitMqProducer(IRabbitMqService rabbitMqService, IOptions<RabbitMqConfig> config)
    {
        _rabbitMqService = rabbitMqService;
        _config = config.Value;
        _channels = _rabbitMqService.GetChannels();
    }

    public void PublishMessage(string message)
    {
        var _channel = _channels[_config.ConsumerThreads];

        // Declare the queue (idempotent)
        _channel.QueueDeclare(
            queue: _config.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        MyMessage m = new MyMessage(message);

        string jsonString = JsonSerializer.Serialize(m);

        var body = Encoding.UTF8.GetBytes(jsonString);

        _channel.BasicPublish(
            exchange: "",
            routingKey: _config.QueueName,
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"Message Published: {message}");



    }
}