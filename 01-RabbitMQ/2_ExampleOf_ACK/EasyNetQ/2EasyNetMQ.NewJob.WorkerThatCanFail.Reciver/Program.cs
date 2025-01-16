using EasyNetQ;

//https://stackoverflow.com/questions/33180108/does-easynetq-support-nack
//https://stackoverflow.com/questions/30914640/how-to-do-error-handling-with-easynetq-rabbitmq

var connectionString = "host=localhost"; // RabbitMQ connection string

using (var bus = RabbitHutch.CreateBus(connectionString))
{
    var queueName = "2EasyNetMQ.NewJob.Queue"; // Replace with your queue name
    bus.Advanced.QueueDeclare(queueName, durable: true, false, false);

    // Subscribe to messages
    await bus.SendReceive.ReceiveAsync<Job>
        (queueName, HandleMessage, x => x.WithAutoDelete(false));

    Console.WriteLine("Press [enter] to exit.");
    Console.ReadLine();
}

static void HandleMessage(Job job)
{
    if (job.ShouldFaillOnWorkerTwo == false)
    {
        // Note: EasyNetQ automatically ACKs messages by default.

        Thread.Sleep(job.HowManySecondsWillJobTake * 500);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" [>] Done");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    else
    {
        // Manually N-ACK the message
        // YOU CAN'T DO THIS WITH EasyNetQ

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" [x] Fail {job.Message}");
        Console.WriteLine($" [x] Fail {job.Type}");
        Console.ForegroundColor = ConsoleColor.Gray;

        // THIS WILL SEND MESSAGE TO DEAD-LETTER QUEQUE
        // DEFAULT NAME OF THIS QUEQUE IS
        // EasyNetQ_Default_Error_Queue
        throw new Exception("JOB FAILED");
    }
}