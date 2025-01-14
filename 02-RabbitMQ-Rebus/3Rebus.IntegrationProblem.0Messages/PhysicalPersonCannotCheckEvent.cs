using _3IntegrationProblem._0Messages;
using System.Text.Json.Serialization;

public class PhysicalPersonCannotCheckEvent : IPhysicalPersonEvents
{
    public Guid Id { get; set; }

    [JsonConstructor]
    public PhysicalPersonCannotCheckEvent(Guid id)
    {
        Id = id;
    }
}