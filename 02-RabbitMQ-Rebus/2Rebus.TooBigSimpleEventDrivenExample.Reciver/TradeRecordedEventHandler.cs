using _2Rebus.TooBigSimpleEventDrivenExample._0Messages;
using Rebus.Handlers;

internal class TradeRecordedEventHandler : IHandleMessages<TradeRecordedEvent>
{
    public TradeRecordedEventHandler()
    {
    }

    public async Task Handle(TradeRecordedEvent message)
    {
        Console.WriteLine
            ($"Invocing trade: {message.Id} ({message.Quantity} x {message.Commodity})");
    }
}
