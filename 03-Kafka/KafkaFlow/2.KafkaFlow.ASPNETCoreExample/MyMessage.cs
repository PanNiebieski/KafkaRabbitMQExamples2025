namespace _2.KafkaFlow.ASPNETCoreExample;

//This will create serialization problems with KafkaFlow

//public class MyMessage
//{
//    public string Text { get; set; } = string.Empty;

//    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

//    public MyMessage(string text, DateTime? timestamp = null)
//    {
//        Text = text;

//        if (timestamp != null)
//            Timestamp = timestamp.Value;
//    }
//}

public record MyMessage(string Text, DateTime Timestamp);