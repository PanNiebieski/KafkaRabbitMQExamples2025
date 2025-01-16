using _2EasyNetMQ.Common;
using EasyNetQ;

Console.WriteLine(AppInfo.Value);

using (var bus = RabbitHutch.CreateBus("host=localhost"))
{
    await bus.SendReceive.ReceiveAsync<TextMessage>("1EasyNetMQ.Queue", HandleTextMessage);

    Console.WriteLine("Listening for messages. Hit <return> to quit.");
    Console.ReadLine();
}

static void HandleTextMessage(TextMessage textMessage)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Got message: {0}", textMessage.Text);
    Console.ResetColor();
}