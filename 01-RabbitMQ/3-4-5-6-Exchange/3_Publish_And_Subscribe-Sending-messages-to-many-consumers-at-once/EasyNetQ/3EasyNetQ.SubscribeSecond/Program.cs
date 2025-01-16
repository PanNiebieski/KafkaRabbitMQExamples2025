using EasyNetQ;
using EasyNetQ.Topology;

using (var bus = RabbitHutch.CreateBus("host=localhost"))
{
    AddCustom(bus);

    await bus.PubSub.SubscribeAsync<string>("3EasyNetQ.SubscribeSecond", HandleTextMessage);

    Console.WriteLine("Listening for messages. Hit <return> to quit.");
    Console.ReadLine();
}

static void HandleTextMessage(string textMessage)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Got message: {0}", textMessage);
    Console.ResetColor();
}

static void AddCustom(IBus bus)
{
    var exchange = bus.Advanced.ExchangeDeclare("3EasyNetQ.PublishToMany.Exchange", ExchangeType.Fanout);
    var queue = bus.Advanced.QueueDeclare("3EasyNetQ.PublishToMany.Queue",
        durable: true, exclusive: false, autoDelete: true);
    bus.Advanced.Bind(exchange, queue, "#");
}