using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

Console.WriteLine(AppInfo.Value);
Console.Title = "1RabbitMQV7.Receive";
Console.WriteLine("1RabbitMQV7.Receive");

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = await factory.CreateConnectionAsync())
using (var channel = await connection.CreateChannelAsync())
{
    await channel.QueueDeclareAsync(queue: "R7.01", durable: false,
        exclusive: false, autoDelete: false, arguments: null);


    Console.WriteLine(" Waiting for messages.");

    var consumer = new AsyncEventingBasicConsumer(channel);

    consumer.ReceivedAsync += async (model, ea) =>
    {
        await Task.Run(() =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.Write("-> Received: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        });
        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };

    await channel.BasicConsumeAsync(queue: "R7.01",
    autoAck: true, consumer: consumer);


    char key = 'z';

    while (key != 'q' && key != 'Q')
    {
        Console.WriteLine(" Press [q] or [Q] to exit.");
        key = Console.ReadKey().KeyChar;
    }

}


