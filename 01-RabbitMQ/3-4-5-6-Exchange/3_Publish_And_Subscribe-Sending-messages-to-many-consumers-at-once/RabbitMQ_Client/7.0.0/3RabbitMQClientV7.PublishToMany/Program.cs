using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = await factory.CreateConnectionAsync())
using (var channel = await connection.CreateChannelAsync())
{
    await channel.ExchangeDeclareAsync(exchange: "R6.03",
        type: ExchangeType.Fanout,
        durable: true, autoDelete: false, arguments: null);

    while (true)
    {
        var message = "Daj komentarz!";
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "R6.03",
            routingKey: "", body: body);

        Console.WriteLine(" [x] Sent {0}", message);
        Console.ReadLine();
    }
}