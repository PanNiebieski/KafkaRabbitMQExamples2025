using System.Text.Json.Serialization;

namespace _3IntegrationProblem._0Messages;

public class ExternalAPIError
{
    public Guid Id { get; set; }

    public string Exception { get; set; }

    public PhysicalPersonRecordedEvent OnWhatEvent { get; set; }

    [JsonConstructor]
    public ExternalAPIError(Guid id, string exception, PhysicalPersonRecordedEvent onWhatEvent)
    {
        Id = id;
        Exception = exception;
        OnWhatEvent = onWhatEvent;
    }
}