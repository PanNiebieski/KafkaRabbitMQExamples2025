using EasyNetQ;
using EasyNetQ.Topology;

using (var bus = RabbitHutch.CreateBus("host=localhost"))
{
    var message = "Daj komentarz!";

    await bus.PubSub.PublishAsync<String>(
            message
        );

    Console.WriteLine(" [x] Sent {0}", message);
    Console.ReadLine();
}


static void AddCustom(IBus bus)
{
    var exchange = bus.Advanced.ExchangeDeclare("3EasyNetQ.PublishToMany.Exchange", ExchangeType.Fanout);
    var queue = bus.Advanced.QueueDeclare("3EasyNetQ.PublishToMany.Queue",
        durable: true, exclusive: false, autoDelete: true);
    bus.Advanced.Bind(exchange, queue, "#");
}
