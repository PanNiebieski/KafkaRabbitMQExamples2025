using System.Text.Json.Serialization;

namespace _3IntegrationProblem._0Messages;

public class PhysicalPersonRecordedEvent : IPhysicalPersonEvents
{
    [JsonConstructor]
    public PhysicalPersonRecordedEvent(Guid id, string firstName, string lastName, string pesel)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Pesel = pesel;
    }

    public Guid Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Pesel { get; set; }
}