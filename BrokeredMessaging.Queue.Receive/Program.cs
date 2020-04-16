using System;

namespace BrokeredMessaging.Queue.Receive
{
    public class Program
    {
        static void Main(string[] args)
        {
            //TODO: Valid Service Bus Connection string and the QueuePath should be provided
            var connectionString = "";
            var queuePath = "demoqueue";


            var receiver = new MessageReceiverService(connectionString, queuePath);
            
            Console.ReadKey();
        }
    }
}
