using Rebus.Activation;
using Rebus.Config;

Console.WriteLine(AppInfo.Value);

using (var act = new BuiltinHandlerActivator())
{
    act.Handle<string>(async message =>
    {
        await Task.Delay(110);
        Console.WriteLine($"Got message : {message}");
    });

    Configure.With(act)
        .Logging(l => l.ColoredConsole())
        .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "0DemoRebus")
                .InputQueueOptions(o => o.SetDurable(true)))
        .Start();

    await Task.Delay(100);
    Console.WriteLine("Press enter to quit");
    Console.ReadLine();
}