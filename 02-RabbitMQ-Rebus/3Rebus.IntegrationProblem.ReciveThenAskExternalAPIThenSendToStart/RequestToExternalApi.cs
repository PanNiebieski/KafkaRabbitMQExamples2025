using _3IntegrationProblem._0Messages;
using System.Text.Json.Serialization;

namespace _3IntegrationProblem.Confirmation
{
    public class RequestToExternalApi
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Pesel { get; set; }

        public RequestToExternalApi(PhysicalPersonRecordedEvent e)
        {
            Id = e.Id;
            FirstName = e.FirstName;
            LastName = e.LastName;
            Pesel = e.Pesel;
        }
    }

    public class ResponseFromExternalApi
    {
        public bool Confirmed { get; set; }

        [JsonConstructor]
        public ResponseFromExternalApi(bool confirmed)
        {
            Confirmed = confirmed;
        }
    }
}