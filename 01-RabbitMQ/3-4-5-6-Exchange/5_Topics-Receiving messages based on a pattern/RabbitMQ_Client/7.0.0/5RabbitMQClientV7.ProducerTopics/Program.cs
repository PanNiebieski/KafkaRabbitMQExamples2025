using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = await factory.CreateConnectionAsync())
using (var channel = await connection.CreateChannelAsync())
{
    await channel.ExchangeDeclareAsync(exchange: "R7.05",
                            type: ExchangeType.Topic);

    Console.WriteLine("Write routingKey:");

    string? usermessageRoutingKey = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(usermessageRoutingKey))
    {
        Environment.ExitCode = 1;
        return;
    }

    var message = "Daj komentarz";

    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchange: "RabbitMqDotNet6Tutorial.05",
                         routingKey: usermessageRoutingKey,
                         body: body);

    Console.WriteLine(" [x] Sent '{0}':'{1}'", usermessageRoutingKey, message);
}