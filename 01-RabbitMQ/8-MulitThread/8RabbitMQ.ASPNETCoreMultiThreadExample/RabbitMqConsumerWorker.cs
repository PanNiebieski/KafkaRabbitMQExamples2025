using _8RabbitMQ.ASPNETCoreMultiThreadExample.Service;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace _8RabbitMQ.ASPNETCoreMultiThreadExample;

public class RabbitMqConsumerWorker : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly string _queueName;
    private readonly int _consumerThreads;
    private InMemoryMessageStore _memoryMessage;
    private readonly List<IModel> _channels;

    private readonly int _number = 0;

    public RabbitMqConsumerWorker(IRabbitMqService rabbitMqService, IOptions<RabbitMqConfig> config, InMemoryMessageStore memoryMessage)
    {
        _rabbitMqService = rabbitMqService;
        _queueName = config.Value.QueueName;
        _consumerThreads = config.Value.ConsumerThreads;
        _memoryMessage = memoryMessage;
        _channels = rabbitMqService.GetChannels();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var threads = new List<Task>();

        for (int i = 0; i < _consumerThreads; i++)
        {
            threads.Add(Task.Run(() => StartConsumer(stoppingToken, i), stoppingToken));
        }
        await Task.WhenAll(threads);
    }


    private void StartConsumer(CancellationToken stoppingToken, int number)
    {
        var item = _channels[number];

        item.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(item);
        consumer.Received += async (model, ea) =>
        {
            await Task.Delay(1000 * number);

            var body = ea.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);

            try
            {
                MyMessage m = JsonSerializer.Deserialize<MyMessage>(jsonString);

                Console.WriteLine($"Received message: {m.Text}");

                _memoryMessage.Write(m, number);

                item.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                item.BasicAck(ea.DeliveryTag, false);
            }

            await Task.Delay(10000 * number);
        };

        item.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            Thread.Sleep(1000 * number);// Keep thread alive
        }
    }
}

