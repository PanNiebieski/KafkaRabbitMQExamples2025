using System.Text;

namespace _7RabbitMQ.ASPNETCoreExample;

public class InMemoryMessageStore
{
    public StringBuilder builder = new StringBuilder();

    private readonly object _lockObject = new();

    public void Write(MyMessage a)
    {
        lock (_lockObject) // Ensure only one thread can access the block at a time
        {
            builder.AppendLine("");
            builder.AppendLine("=================================");
            builder.AppendLine(a.Text);
            builder.AppendLine(a.Timestamp.ToLongTimeString());
            builder.AppendLine("=================================");
            builder.AppendLine("");
        }
    }

    public string Read()
    {
        lock (_lockObject) // Synchronize read operations
        {
            return builder.ToString();
        }
    }
}