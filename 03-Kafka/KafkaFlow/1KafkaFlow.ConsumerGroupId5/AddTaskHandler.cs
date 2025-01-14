using KafkaFlow;
using Microsoft.Extensions.Logging;

public class AddTaskHandler : IMessageHandler<AddTaskRequest>
{
    private readonly ILogger<AddTaskHandler> _logger;

    public AddTaskHandler(ILogger<AddTaskHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, AddTaskRequest message)
    {
        if (message.DueDate.HasValue)
            _logger.LogInformation("New Task {Title} scheduled to {DueDate}",
                message.Title,
                message.DueDate);

        return Task.CompletedTask;
    }
}