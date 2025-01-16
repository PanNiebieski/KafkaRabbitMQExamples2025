using System.Text.Json.Serialization;

namespace _2Rebus.TooBigSimpleEventDrivenExample._0Messages;

public class UserLoggedEvent
{
    private Guid id;
    private string username;

    [JsonConstructor]
    public UserLoggedEvent(Guid id, string username)
    {
        this.Id = id;
        this.UserName = username;
    }

    public Guid Id { get => id; set => id = value; }
    public string UserName { get => username; set => username = value; }
}