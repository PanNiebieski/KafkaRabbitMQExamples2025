using _2Rebus.TooBigSimpleEventDrivenExample._0Messages;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;

Console.WriteLine(AppInfo.Value);

using (var activator = new BuiltinHandlerActivator())
{
    var bus = Configure.With(activator)
        .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://guest:guest@localhost:5672"))
        .Start();

    await Task.Delay(500);

    Console.WriteLine("Trading is running");

    while (true)
    {
        Console.WriteLine(@"
            a) Publish TradeRecordedEvent
            b) Publish DocumentSavedEvent
            c) Publish UserLogedEvent
            q) Quit");

        var keyChar = char.ToLower(Console.ReadKey(true).KeyChar);

        switch (keyChar)
        {
            case 'a':
                await SendTradeRecordedEventAsync(bus);
                break;
            case 'b':
                await SendDocumentSavedAsync(bus);
                break;
            case 'c':
                await SendUserLoggedEventAsync(bus);
                break;
            case 'q':
                break;
            default:
                Console.WriteLine("There's no option ({0})", keyChar);
                break;
        }
    }

}




async Task SendTradeRecordedEventAsync(IBus bus)
{
    Console.WriteLine("Please enter new trade details");
    Console.WriteLine(" commodity > ");
    var commodity = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(commodity)) return;

    int quantity;
    Console.WriteLine(" quantity > ");
    while (!int.TryParse(Console.ReadLine(), out quantity)) ;

    var tradeEvent = new TradeRecordedEvent(Guid.NewGuid(), commodity, quantity);
    await bus.Publish(tradeEvent);

    Console.WriteLine("Sended tradeEvent");
}

async Task SendDocumentSavedAsync(IBus bus)
{
    Console.WriteLine("Please enter new document details");
    Console.WriteLine(" document name > ");
    var documentName = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(documentName)) return;

    var docEvent = new DocumentSavedEvent(Guid.NewGuid(), documentName);
    await bus.Publish(docEvent);

    Console.WriteLine("Sended docEvent");
}

async Task SendUserLoggedEventAsync(IBus bus)
{
    Console.WriteLine("Please enter new trade details");
    Console.WriteLine(" Username > ");
    var username = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(username)) return;

    var userEvent = new UserLoggedEvent(Guid.NewGuid(), username);
    await bus.Publish(userEvent);

    Console.WriteLine("Sended userEvent");
}