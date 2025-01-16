namespace _5EasyNetQ.Common;

public interface IPayment
{
    decimal Amount { get; set; }
}

public class CardPayment : IPayment
{
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public string ExpiryDate { get; set; }

    // Interface implementation
    public decimal Amount { get; set; }
}

public class PurchaseOrder : IPayment
{
    public string PoNumber { get; set; }
    public string CompanyName { get; set; }
    public int PaymentDayTerms { get; set; }

    // Interface implementation
    public decimal Amount { get; set; }
}