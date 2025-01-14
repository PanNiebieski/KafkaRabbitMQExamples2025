namespace _7EasyNetMQ.ASPNETCoreExample;

public class MyMessage
{
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}