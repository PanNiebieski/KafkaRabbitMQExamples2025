using _2Rebus.TooBigSimpleEventDrivenExample._0Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;

Console.WriteLine(AppInfo.Value);

using (var activator = new BuiltinHandlerActivator())
{
    activator.Register(() => new TradeRecordedEventHandler());
    activator.Register(() => new UserLoggedEventHandler());
    activator.Register(() => new DocumentSavedEventHandler());

    var bus = Configure.With(activator)
        .Logging(l => l.ColoredConsole(minLevel: Rebus.Logging.LogLevel.Warn))
        .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "Rebus.SystemTooBig.Trading"))
        .Routing(r => r.TypeBased().MapAssemblyOf<TradeRecordedEvent>("Rebus.SystemTooBig.Trading"))
        .Start();

    await bus.Subscribe<TradeRecordedEvent>();
    await bus.Subscribe<UserLoggedEvent>();
    await bus.Subscribe<DocumentSavedEvent>();

    Console.WriteLine("Invoicing is running - press Enter to quite");
    Console.ReadKey();
}