namespace _8RabbitMQ.ASPNETCoreMultiThreadExample;

public class MyMessage
{
    public string Text { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public MyMessage(string text)
    {
        Text = text;
    }
}