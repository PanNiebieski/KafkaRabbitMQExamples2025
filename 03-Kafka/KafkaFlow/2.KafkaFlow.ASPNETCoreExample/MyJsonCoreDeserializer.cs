using _2.KafkaFlow.ASPNETCoreExample;
using KafkaFlow;
using System.Text;
using System.Text.Json;

public class MyJsonCoreDeserializer : IDeserializer
{
    public object Deserialize(byte[] data, IMessageContext context)
    {
        return JsonSerializer.Deserialize<MyMessage>(Encoding.UTF8.GetString(data));
    }

    public async Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        try
        {
            // Ensure the stream is at the beginning
            input.Seek(0, SeekOrigin.Begin);

            // Deserialize the JSON from the stream
            var result = await JsonSerializer.DeserializeAsync(input, type);

            return result ?? throw new InvalidOperationException("Deserialization resulted in a null object.");
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"An error occurred during deserialization: {ex.Message}");
            throw;
        }
    }
}