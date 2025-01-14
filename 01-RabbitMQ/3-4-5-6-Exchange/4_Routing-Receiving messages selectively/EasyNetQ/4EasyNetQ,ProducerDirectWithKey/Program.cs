using EasyNetQ;
using EasyNetQ.Topology;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Channels;

var advancedBus = RabbitHutch.CreateBus("host=localhost").Advanced;

var queue = advancedBus.QueueDeclare("4EasyNetQ,ProducerDirectWithKey.Queue", true, false, false);
var exchange = advancedBus.ExchangeDeclare("4EasyNetQ,ProducerDirectWithKey.Exchange", ExchangeType.Direct);


while (true)
{
    Console.WriteLine("Write what you want to send");
    Console.WriteLine("Write nothing to exit.");

    string usermessage = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(usermessage))
        break;

    Console.WriteLine("Choose your routingKey");
    Console.WriteLine("1. Email");
    Console.WriteLine("2. Mail");
    Console.WriteLine("3. Test");

    char userRoutingKey = Console.ReadKey().KeyChar;
    int userRoutingNumber = int.Parse(userRoutingKey.ToString());

    if (userRoutingNumber == 1 || userRoutingNumber == 2 || userRoutingNumber == 3)
    {
        RoutingKey key = (RoutingKey)int.Parse(userRoutingKey.ToString());
        var routingKey = key.GetDescription<RoutingKey>();

        var binding = advancedBus.Bind(exchange, queue, routingKey);

        await advancedBus.PublishAsync<string>(exchange, routingKey, true,
            new Message<string>(usermessage));

        WriteMessageOnConsole($"{usermessage}:{key.GetDescription<RoutingKey>()}");

    }
}

advancedBus.Dispose();


void WriteMessageOnConsole(string message)
{
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\tSent {0}", message);
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("");
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
