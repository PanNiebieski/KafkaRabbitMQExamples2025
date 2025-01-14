using RabbitMQ.Client;
using System.Text;

Console.WriteLine(AppInfo.Value);
Console.Title = "1RabbitMQV7.Send";
Console.WriteLine("1RabbitMQV7.Send");


var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = await factory.CreateConnectionAsync())
using (var channel = await connection.CreateChannelAsync())
{
    await channel.QueueDeclareAsync(queue: "R7.01",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    while (true)
    {


        Console.WriteLine("Write what you want to send");
        Console.WriteLine("Write nothing to exit.");

        string? usermessage = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(usermessage))
            break;

        string message = usermessage;
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "",
            routingKey: "R7.01", body);


        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\tSent {0}", message);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("");
    }


}

