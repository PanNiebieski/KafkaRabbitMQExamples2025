using _7RabbitMQ.ASPNETCoreExample.Service;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace _7RabbitMQ.ASPNETCoreExample;

public class RabbitMqConsumerWorker : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly string _queueName;
    private InMemoryMessageStore _memoryMessage;
    private IChannel _channel;

    private readonly int _number = 0;

    public RabbitMqConsumerWorker(IRabbitMqService rabbitMqService, IOptions<RabbitMqConfig> config, InMemoryMessageStore memoryMessage)
    {
        _rabbitMqService = rabbitMqService;

        _queueName = config.Value.QueueName;
        _memoryMessage = memoryMessage;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await _rabbitMqService.GetChannelAsync();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);

            MyMessage m = JsonSerializer.Deserialize<MyMessage>(jsonString);

            Console.WriteLine($"Received message: {m.Text}");

            _memoryMessage.Write(m);

            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await _channel.BasicConsumeAsync(queue: _queueName,
                             autoAck: false,
                             consumer: consumer);

        while (true)
        {
            await Task.Delay(5000, stoppingToken);
            Console.WriteLine($"RabbitMqConsumerWorker Still waiting");
        }
    }
}

