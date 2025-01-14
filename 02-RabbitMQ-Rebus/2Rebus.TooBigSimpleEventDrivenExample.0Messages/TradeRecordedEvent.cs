using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _2Rebus.TooBigSimpleEventDrivenExample._0Messages;

public class TradeRecordedEvent
{
    private Guid id;
    private string commodity;
    private int quantity;

    [JsonConstructor]
    public TradeRecordedEvent(Guid id, string commodity, int quantity)
    {
        this.Id = id;
        this.Commodity = commodity;
        this.Quantity = quantity;
    }

    public Guid Id { get => id; set => id = value; }
    public string Commodity { get => commodity; set => commodity = value; }
    public int Quantity { get => quantity; set => quantity = value; }
}