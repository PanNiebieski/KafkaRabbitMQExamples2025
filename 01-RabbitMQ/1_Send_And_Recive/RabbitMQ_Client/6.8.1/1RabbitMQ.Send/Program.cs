﻿using RabbitMQ.Client;
using System.Text;

Console.Title = "1RabbitMQ.Send";
Console.WriteLine("1RabbitMQ.Send");

var factory = new ConnectionFactory() { HostName = "localhost" };
Console.WriteLine(AppInfo.Value);

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "R6.01",
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

        channel.BasicPublish(exchange: "",
            routingKey: "R6.01",
            basicProperties: null, body);

        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\tSent {0}", message);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("");
    }

}