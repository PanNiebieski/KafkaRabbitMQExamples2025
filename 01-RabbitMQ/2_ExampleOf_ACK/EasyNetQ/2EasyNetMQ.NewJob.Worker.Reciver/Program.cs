using EasyNetQ;

//https://stackoverflow.com/questions/33180108/does-easynetq-support-nack
//https://stackoverflow.com/questions/30914640/how-to-do-error-handling-with-easynetq-rabbitmq
//https://stackoverflow.com/questions/56629803/how-to-accept-and-reject-ack-and-nack-messages-manually
//https://stackoverflow.com/questions/60316265/easynetq-handle-manual-ack

var connectionString = "host=localhost"; // RabbitMQ connection string

using (var bus = RabbitHutch.CreateBus(connectionString))
{
    var queueName = "2EasyNetMQ.NewJob.Queue"; // Replace with your queue name
    bus.Advanced.QueueDeclare(queueName, durable: true, false, false);

    await bus.SendReceive.ReceiveAsync<Job>
        (queueName, HandleMessage, x => x.WithAutoDelete(false));

    Console.WriteLine("Press [enter] to exit.");
    Console.ReadLine();
}

static void HandleMessage(Job job)
{
    try
    {
        // Process the message
        Console.WriteLine(" [>] Received {0}", job.Message);
        Console.WriteLine(" [>] Received {0}", job.Type);
        Thread.Sleep(job.HowManySecondsWillJobTake * 1000);

        // Manually ACK the message
        // YOU CAN'T DO THIS WITH EasyNetQ
        // Note: EasyNetQ automatically ACKs messages by default.

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" [>] Done");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing message: {ex.Message}");
        // Handle any exceptions or log errors here
    }
}