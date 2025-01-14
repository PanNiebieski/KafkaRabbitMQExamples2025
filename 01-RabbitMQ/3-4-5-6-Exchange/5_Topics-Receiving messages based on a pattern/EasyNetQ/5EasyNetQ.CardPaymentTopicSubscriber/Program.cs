using _5EasyNetQ.Common;
using EasyNetQ;

using (var bus = RabbitHutch.CreateBus("host=localhost"))
{
    bus.PubSub.Subscribe<IPayment>("cards", Handler, x => x.WithTopic("payment.cardpayment"));

    Console.WriteLine("Listening for (payment.cardpayment) messages. Hit <return> to quit.");
    Console.ReadLine();
}

static void Handler(IPayment payment)
{
    var cardPayment = payment as CardPayment;

    if (cardPayment != null)
    {
        Console.WriteLine("Processing Card Payment = <" +
                          cardPayment.CardNumber + ", " +
                          cardPayment.CardHolderName + ", " +
                          cardPayment.ExpiryDate + ", " +
                          cardPayment.Amount + ">");
    }
}