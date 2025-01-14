using _3IntegrationProblem._0Messages;
using System.Text.Json.Serialization;

public class PhysicalPersonRejectedEvent : IPhysicalPersonEvents
{
    public Guid Id { get; set; }

    [JsonConstructor]
    public PhysicalPersonRejectedEvent(Guid id)
    {
        Id = id;
    }
}