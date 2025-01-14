using _3IntegrationProblem._0Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using _3IntegrationProblem.Confirmation;
using Rebus.Routing.TypeBased;
using Rebus.Retry.Simple;
using Rebus.Persistence.FileSystem;

Console.Title = "This send message to IntegrationProblem.ExternalThirdAPI";
Console.WriteLine(AppInfo.Value);

using var activator = new BuiltinHandlerActivator();
using var http = new HttpClient();

activator.Register((bus, context) => new PhysicalPersonRecordedEventHandler2(bus, http));

Configure.With(activator)
    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Error))
    .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", 
            "3IntegrationProblem.Server.Reciver.Confirmation"))
    .Routing(r => r.TypeBased()
        .Map<PhysicalPersonRecordedEvent>("3IntegrationProblem.Server.Reciver.Confirmation"))

    .Options(b => b.SimpleRetryStrategy(maxDeliveryAttempts:1,secondLevelRetriesEnabled: true))
    .Timeouts(t =>
    {
        t.UseFileSystem(AppInfo.PathToTimeOut);
    })

    .Start();

await activator.Bus.Subscribe<PhysicalPersonRecordedEvent>();

Console.WriteLine("This will recive PhysicalPersonRecordedEvent");
Console.WriteLine("This send message to IntegrationProblem.ExternalThirdAPI");




while (true)
{
    Console.ReadKey();
}
