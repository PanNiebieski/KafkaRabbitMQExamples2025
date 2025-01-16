using KafkaFlow;
using System.Text;
using System.Text.Json;

public class MyJsonCoreSerializer : ISerializer
{
    public byte[] Serialize(object message, IMessageContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
    }

    public async Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
        try
        {
            // Ensure the stream is at the beginning
            output.Seek(0, SeekOrigin.Begin);

            // Serialize the message to the stream
            await JsonSerializer.SerializeAsync(output, message);

            // Optionally flush the stream
            await output.FlushAsync();
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"An error occurred during serialization: {ex.Message}");
            throw;
        }
    }
}