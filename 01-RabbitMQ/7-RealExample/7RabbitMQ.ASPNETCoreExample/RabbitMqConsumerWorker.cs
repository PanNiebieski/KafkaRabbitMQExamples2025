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
    private readonly IModel _channel;

    private readonly int _number = 0;

    public RabbitMqConsumerWorker(IRabbitMqService rabbitMqService, IOptions<RabbitMqConfig> config, InMemoryMessageStore memoryMessage)
    {
        _rabbitMqService = rabbitMqService;
        _queueName = config.Value.QueueName;
        _memoryMessage = memoryMessage;
        _channel = rabbitMqService.GetChannel();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);

            MyMessage m = JsonSerializer.Deserialize<MyMessage>(jsonString);

            Console.WriteLine($"Received message: {m.Text}");

            _memoryMessage.Write(m);

            //File.WriteAllText($@"D:\Messages\{_number}.txt", message);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName,
                             autoAck: false,
                             consumer: consumer);

        while (true)
        {
            await Task.Delay(5000, stoppingToken);
            Console.WriteLine($"RabbitMqConsumerWorker Still waiting");
        }
    }
}