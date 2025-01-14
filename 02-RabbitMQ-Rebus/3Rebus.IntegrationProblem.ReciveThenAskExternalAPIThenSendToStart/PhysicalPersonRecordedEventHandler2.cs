using _3IntegrationProblem._0Messages;
using Rebus.Bus;
using Rebus.Exceptions;
using Rebus.Handlers;
using Rebus.Messages;
using Rebus.Retry.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3IntegrationProblem.Confirmation
{
    public class PhysicalPersonRecordedEventHandler2 : 
        IHandleMessages<PhysicalPersonRecordedEvent>, IHandleMessages<IFailed<PhysicalPersonRecordedEvent>>
    {
        private IBus _bus;
        private HttpClient _http;

        public PhysicalPersonRecordedEventHandler2(IBus bus, HttpClient http)
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

                var response = await _http.PostAsync("https://localhost:7089/check", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

            desResponse = JsonSerializer.Deserialize<ResponseFromExternalApi>
                                                   (responseContent, options);
            Console.WriteLine("");
            if (desResponse.Confirmed)
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

        public async Task Handle(IFailed<PhysicalPersonRecordedEvent> failed)
        {
            var message = failed.Message;

            await _bus.Publish(new PhysicalPersonCannotCheckEvent(message.Id));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Error - CannotCheck");

            await _bus.Publish(new ExternalAPIError(message.Id,
                failed.ErrorDescription,
                message));


            const int maxDeferCount = 5;
            var deferCount = Convert.ToInt32(failed.Headers.GetValueOrDefault(Headers.DeferCount));
            if (deferCount >= maxDeferCount)
            {
                await _bus.Advanced.TransportMessage.Deadletter($"Failed after {deferCount} deferrals\n\n" +
                    $"{failed.ErrorDescription}");
                return;
            }
            await _bus.Advanced.TransportMessage.Defer(TimeSpan.FromSeconds(30));
        }
    }
}
