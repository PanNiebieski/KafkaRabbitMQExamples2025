using _3IntegrationProblem._0Messages;
using System.Text.Json.Serialization;

public class PhysicalPersonApprovedEvent : IPhysicalPersonEvents
{
    public Guid Id { get; set; }

    [JsonConstructor]
    public PhysicalPersonApprovedEvent(Guid id)
    {
        Id = id;
    }
}