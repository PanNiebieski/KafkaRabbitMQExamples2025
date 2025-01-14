using _2Kafka.Confluent.ASPNETCoreExample;
using KafkaFlow;
using System.Text.Json;

namespace _2.KafkaFlow.ASPNETCoreExample;

public class KafkaFlowMessageHandler : IMessageHandler<MyMessage>
{
    //private readonly InMemoryMessageStore _messageStore;

    //public KafkaFlowMessageHandler(InMemoryMessageStore messageStore)
    //{
    //    _messageStore = messageStore;
    //}

    public async Task Handle(IMessageContext context, MyMessage myMessage)
    {
        //var myMessage = JsonSerializer.Deserialize<MyMessage>(message);
        //_messageStore.Write(myMessage);

        Console.WriteLine($"Consumed message: {myMessage.Text}");

        await Task.CompletedTask; // Simulate async processing
    }
}