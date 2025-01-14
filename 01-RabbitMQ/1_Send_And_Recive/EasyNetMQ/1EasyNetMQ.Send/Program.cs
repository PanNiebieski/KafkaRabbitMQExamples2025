using _2EasyNetMQ.Common;
using EasyNetQ;

Console.WriteLine(AppInfo.Value);

using (var bus = RabbitHutch.CreateBus("host=localhost"))
{
    for (int i = 0; i < 10; i++)
    {
        await bus.SendReceive.SendAsync("1EasyNetMQ.Queue",
            new TextMessage()
            { Text = i + ": Hello World from EasyNetQ" }
        );

    }
}



//static void AddCustom(IBus bus)
//{
//    var exchange = bus.Advanced.ExchangeDeclare("2EasyNetMQ.RabbitMqDotNetTutorial.01.Exchange", ExchangeType.Direct);
//    var queue = bus.Advanced.QueueDeclare("2EasyNetMQ.RabbitMqDotNetTutorial.01.Queue",
//        durable: false, exclusive: false, autoDelete: true);
//    bus.Advanced.Bind(exchange, queue, "#");
//}

//static async Task Alternative(IBus bus, int i)
//{
//    var exchange = bus.Advanced.ExchangeDeclare("2EasyNetMQ.RabbitMqDotNetTutorial.01.Exchange", ExchangeType.Direct);

//    await bus.Advanced.PublishAsync(exchange,
//        "2EasyNetMQ.RabbitMqDotNetTutorial.01.Queue", false,
//        new TextMessage2()
//        { Text = i + ": Hello World from EasyNetQ" });
//}