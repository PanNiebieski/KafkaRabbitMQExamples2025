using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = await factory.CreateConnectionAsync())
using (var channel = await connection.CreateChannelAsync())
{
    await channel.ExchangeDeclareAsync(exchange: "RabbitMqDotNet6Tutorial.03",
        type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

    var queueName = (await channel.QueueDeclareAsync()).QueueName;
    await channel.QueueBindAsync(queue: queueName, exchange: "RabbitMqDotNet6Tutorial.03",
        routingKey: "");

    Console.WriteLine(" [*] Waiting for logs.");

    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.ReceivedAsync += async (model, ea) =>
    {
        await Task.Run(() =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] {0}", message);
        });
    };

    await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}