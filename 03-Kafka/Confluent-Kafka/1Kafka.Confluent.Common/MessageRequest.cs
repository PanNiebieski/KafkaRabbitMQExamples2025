namespace _1Kafka.Confluent.Common;

public class MessageRequest
{
    public int Id { get; set; }

    public Guid UniqueId { get; set; }

    public int Color { get; set; }

    public string Message { get; set; }

    public DateTime StartProducer { get; set; }
}

