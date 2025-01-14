using KafkaFlow;

public class StatisticsMiddleware : IMessageMiddleware
{
    private static int _total = 0;
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var batch = context.GetMessagesBatch();

        _total += batch.Count;

        Console.WriteLine($"Current Total: {_total}");

        await next(context);
    }
}