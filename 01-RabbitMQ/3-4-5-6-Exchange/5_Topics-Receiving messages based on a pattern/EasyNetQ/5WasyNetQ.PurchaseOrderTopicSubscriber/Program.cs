using _5EasyNetQ.Common;
using EasyNetQ;

using (var bus = RabbitHutch.CreateBus("host=localhost"))
{
    bus.PubSub.Subscribe<IPayment>("purchaseorders", Handler, x => x.WithTopic("payment.purchaseorder"));

    Console.WriteLine("Listening for (payment.purchaseorer) messages. Hit <return> to quit.");
    Console.ReadLine();
}

static void Handler(IPayment payment)
{
    var purchaseOrder = payment as PurchaseOrder;

    if (purchaseOrder != null)
    {
        Console.WriteLine("Processing Purchase Order = <" +
                          purchaseOrder.CompanyName + ", " +
                          purchaseOrder.PoNumber + ", " +
                          purchaseOrder.PaymentDayTerms + ", " +
                          purchaseOrder.Amount + ">");
    }
}