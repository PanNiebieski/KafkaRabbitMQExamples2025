using EasyNetQ;

namespace _2EasyNetMQ.Common;

public class TextMessage
{
    public string Text { get; set; }
}


public class TextMessage2 : IMessage
{
    public string Text { get; set; }

    public MessageProperties Properties => new();

    public Type MessageType => GetType();

    public object GetBody() => this;

}

