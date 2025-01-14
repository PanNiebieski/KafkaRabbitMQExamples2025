using EasyNetQ;
using Newtonsoft.Json;

while (true)
{
    Console.WriteLine("Write [.] to increase time to do job for worker. Foreach [.]");
    Console.WriteLine("Write [!] at least once. To make one worker fail job");
    Console.WriteLine("Write [q] or [Q] to exit.");

    string whatuserwrote = Console.ReadLine();

    if (whatuserwrote == "q" || whatuserwrote == "Q")
        break;
    if (string.IsNullOrEmpty(whatuserwrote))
        continue;

    Job job = CreateJob(whatuserwrote);

    using (var bus = RabbitHutch.CreateBus("host=localhost"))
    {

        await bus.SendReceive.SendAsync<Job>("2EasyNetMQ.NewJob.Queue"
            , job
        );
    }

    string message = JsonConvert.SerializeObject(job);
    WriteMessageOnConsole(message);
}


Job CreateJob(string usertext)
{
    int howManySecondsWillJobTake = usertext.Split('.').Length - 1;
    bool shouldFail = usertext.IndexOf('!') > 0;

    string message = usertext.Replace("!", "")
        .Replace(".", "");

    return new Job()
    {
        Message = message,
        HowManySecondsWillJobTake = howManySecondsWillJobTake,
        Type = JobType.SendEmail,
        ShouldFaillOnWorkerTwo = shouldFail

    };
}

void WriteMessageOnConsole(string message)
{
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\tSent {0}", message);
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("");
}
