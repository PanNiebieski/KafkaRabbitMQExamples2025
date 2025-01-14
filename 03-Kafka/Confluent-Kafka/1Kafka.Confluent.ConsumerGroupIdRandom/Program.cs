
using _1Kafka.Confluent.Common;
using Confluent.Kafka;
using System.Net;
using System.Text.Json;

int unique = new Random().Next(6, int.MaxValue);

string topic = "1Kafka.Confluent.Test";
string groupId = "1Kafka.Confluent.Test_Group_" + unique.ToString();
string bootstrapServers = "localhost:19092";

Console.WriteLine($"{topic} GROUP ID : {groupId}");

var config = new ConsumerConfig
{
	GroupId = groupId,
	BootstrapServers = bootstrapServers,
	AutoOffsetReset = AutoOffsetReset.Earliest
};

try
{
	using (var consumerBuilder = new ConsumerBuilder
	<Ignore, string>(config).Build())
	{
		consumerBuilder.Subscribe(topic);
		var cancelToken = new CancellationTokenSource();

		try
		{
			while (true)
			{
				var consumer = consumerBuilder.Consume
				   (cancelToken.Token);
				var messageRequest = JsonSerializer.Deserialize
					<MessageRequest>
						(consumer.Message.Value);

				Console.WriteLine($"\nProducerStarted: {messageRequest.StartProducer}");
				Console.WriteLine($"\nProcessing Number: {messageRequest.Id}");
				Console.WriteLine($"Processing UniqueId: {messageRequest.UniqueId}");
				if (messageRequest.Color >= 0 && messageRequest.Color <= 15)
					Console.ForegroundColor = (ConsoleColor)messageRequest.Color;
				Console.WriteLine(messageRequest.Message);
				Console.ResetColor();
				Console.WriteLine("");
			}
		}
		catch (OperationCanceledException)
		{
			consumerBuilder.Close();
		}
	}
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}