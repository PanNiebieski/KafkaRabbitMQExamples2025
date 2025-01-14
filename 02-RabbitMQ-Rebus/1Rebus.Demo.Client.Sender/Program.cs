using Rebus.Activation;
using Rebus.Config;

Console.WriteLine(AppInfo.Value);

using (var activator = new BuiltinHandlerActivator())
{
    var bus = Configure.With(activator)
        .Logging(l => l.ColoredConsole())
        .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://guest:guest@localhost:5672")
            .InputQueueOptions(o => o.SetDurable(true)))
        .Start();

    await Task.Delay(500);

    while (true)
    {
        Console.Write("Type something >");
        var text = Console.ReadLine();

        if (string.IsNullOrEmpty(text)) break;

        //Rebus allows us to define a destination at the point of sending a message:
        await bus.Advanced.Routing.Send("0DemoRebus", text);
        //However, this doesn’t scale particularly well.
        //Instead, the recommended approach is to route messages based on their type, which is the default configuration.

        //bus.Publish(text).Wait();
    }
}