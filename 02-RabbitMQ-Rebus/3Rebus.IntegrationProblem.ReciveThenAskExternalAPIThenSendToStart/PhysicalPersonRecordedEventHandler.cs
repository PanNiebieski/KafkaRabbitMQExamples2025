using _3IntegrationProblem._0Messages;
using Rebus.Bus;
using Rebus.Handlers;
using System.Text;
using _3IntegrationProblem.Confirmation;
using System.Text.Json;

public class PhysicalPersonRecordedEventHandler : IHandleMessages<PhysicalPersonRecordedEvent>
{
    private IBus _bus;
    private HttpClient _http;

    public PhysicalPersonRecordedEventHandler(IBus bus, HttpClient http)
    {
        this._bus = bus;
        this._http = http;
    }

    public async Task Handle(PhysicalPersonRecordedEvent message)
    {
        Console.Write(@$"PhysicalPerson needs confirmation
                    {message.FirstName} {message.LastName} {message.Pesel}");

        var request = new RequestToExternalApi(message);
        string jsonString = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        ResponseFromExternalApi desResponse = null;

        try
        {
            var response = await _http.PostAsync("https://localhost:7089/check", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            desResponse = JsonSerializer.Deserialize<ResponseFromExternalApi>
                                                        (responseContent, options);
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.Message);
            await _bus.Publish(new ExternalAPIError(message.Id, ex.Message, message));
            //throw;
            // OK ReBus can send this message to error que by itself
        }

        Console.WriteLine("");
        if (desResponse == null)
        {
            await _bus.Publish(new PhysicalPersonCannotCheckEvent(message.Id));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Error - CannotCheck");
        }
        else if (desResponse.Confirmed)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("APPROVED");
            await _bus.Publish(new PhysicalPersonApprovedEvent(message.Id));
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("REJECTED");
            await _bus.Publish(new PhysicalPersonRejectedEvent(message.Id));
        }
        Console.ResetColor();

    }
}