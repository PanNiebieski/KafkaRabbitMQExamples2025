using EasyNetQ;
using EasyNetQ.Topology;
using System.ComponentModel;
using System.Reflection;

var bus = RabbitHutch.CreateBus("host=localhost");
var advancedBus = bus.Advanced;

var queue = advancedBus.QueueDeclare("4EasyNetQ,ProducerDirectWithKey.Queue", true, false, false);
var exchange = advancedBus.ExchangeDeclare("4EasyNetQ,ProducerDirectWithKey.Exchange", ExchangeType.Direct);

RoutingKey key1 = RoutingKey.Email;

var binding1 = advancedBus.Bind(exchange, queue, key1.GetDescription<RoutingKey>());

advancedBus.Consume<string>(queue, Handler);

Console.ReadLine();

advancedBus.Dispose();

void Handler(IMessage<string> message, MessageReceivedInfo info)
{
    var routingKey = info.RoutingKey;
    Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
}

public enum RoutingKey
{
    [Description("Email")]
    Email = 1,

    [Description("Mail")]
    Mail = 2,

    [Description("Test")]
    Test = 3
}

public static class Helper
{
    public static string GetDescription<T>(this T enumerationValue)
    where T : struct
    {
        Type type = enumerationValue.GetType();
        if (!type.IsEnum)
        {
            throw new ArgumentException
                ("EnumerationValue must be of Enum type", "enumerationValue");
        }

        //Tries to find a DescriptionAttribute for a potential friendly name
        //for the enum
        MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
        if (memberInfo != null && memberInfo.Length > 0)
        {
            object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                //Pull out the description value
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }
        //If we have no description attribute, just return the ToString of the enum
        return enumerationValue.ToString();
    }
}