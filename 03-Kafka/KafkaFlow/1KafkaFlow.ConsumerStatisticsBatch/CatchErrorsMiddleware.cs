using KafkaFlow;
using System.Text;

public class CatchErrorsMiddleware : IMessageMiddleware
{
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
        }
    }
}

public class ReadMiddleware : IMessageMiddleware
{
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var bytes = context.Message.Value as byte[];
        var message = Encoding.UTF8.GetString(bytes);

        Console.WriteLine($"Read message: {message}");
        await next(context);
    }
}