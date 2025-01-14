using _2Rebus.TooBigSimpleEventDrivenExample._0Messages;
using Rebus.Handlers;

internal class UserLoggedEventHandler : IHandleMessages<UserLoggedEvent>
{
    public UserLoggedEventHandler()
    {
    }

    public async Task Handle(UserLoggedEvent message)
    {
        Console.WriteLine
            ($"User was logged: {message.Id} ({message.UserName})");
    }
}
