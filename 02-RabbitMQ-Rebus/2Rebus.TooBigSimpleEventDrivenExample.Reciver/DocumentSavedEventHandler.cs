using _2Rebus.TooBigSimpleEventDrivenExample._0Messages;
using Rebus.Handlers;

internal class DocumentSavedEventHandler : IHandleMessages<DocumentSavedEvent>
{
    public DocumentSavedEventHandler()
    {
    }

    public async Task Handle(DocumentSavedEvent message)
    {
        Console.WriteLine
            ($"Document was saved: {message.Id} ({message.FileName})");
    }
}