using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = await factory.CreateConnectionAsync())
using (var channel = await connection.CreateChannelAsync())
{
    await channel.ExchangeDeclareAsync(exchange: "RabbitMqDotNet6Tutorial.05",
        type: ExchangeType.Topic);
    var queueName = (await channel.QueueDeclareAsync()).QueueName;

    Console.WriteLine("Write what you want to RECVIE");

    string? usermessageBindingKey = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(usermessageBindingKey))
    {
        Environment.ExitCode = 1;
        return;
    }

    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                                    usermessageBindingKey);


    await channel.QueueBindAsync(queue: queueName,
                          exchange: "R7.05",
                          routingKey: usermessageBindingKey);


    Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.ReceivedAsync += async (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var routingKey = ea.RoutingKey;
        Console.WriteLine(" [x] Received '{0}':'{1}'",
                          routingKey,
                          message);
    };
    await channel.BasicConsumeAsync(queue: queueName,
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}