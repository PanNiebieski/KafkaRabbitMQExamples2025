using _3IntegrationProblem._0Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;

Console.WriteLine(AppInfo.Value);

using (var activator = new BuiltinHandlerActivator())
{
    Console.Title = "Error Reciver is waiting for errors";

    activator.Register(() => new ExternalAPIErrorHandler());

    var bus = Configure.With(activator)
        .Logging(l => l.ColoredConsole(minLevel: Rebus.Logging.LogLevel.Error))
        .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "3IntegrationProblem.Server.Reciver.Confirmation.Error"))
        .Routing(r => r.TypeBased()
        .Map<ExternalAPIError>("3IntegrationProblem.Server.Reciver.Confirmation.Error"))
        .Start();

    await activator.Bus.Subscribe<ExternalAPIError>();

    await Task.Delay(500);

    Console.WriteLine("Error Reciver is waiting for errors");
    Console.ReadKey();
}