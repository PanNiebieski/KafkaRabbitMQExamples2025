using _3IntegrationProblem._0Messages;
using Rebus.Handlers;

internal class ExternalAPIErrorHandler : IHandleMessages<ExternalAPIError>
{
    public ExternalAPIErrorHandler()
    {
    }

    public async Task Handle(ExternalAPIError message)
    {
        Console.WriteLine("");
        Console.WriteLine("=======================");
        Console.WriteLine($"PhysicalPerson get error {message.OnWhatEvent.FirstName} {message.OnWhatEvent.LastName} {message.OnWhatEvent.Pesel}");
        Console.WriteLine(message.Exception);
        Console.WriteLine($"Id : {message.OnWhatEvent.Id}");
        Console.WriteLine("=======================");
        Console.WriteLine("");
    }
}