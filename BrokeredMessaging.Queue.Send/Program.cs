namespace BrokeredMessaging.Queue.Send
{
    internal class BrokeredMessagingSender
    {
        private static void Main(string[] args)
        {
            //TODO: Valid Service Bus Connection string and the QueuePath should be provided
            var connectionString ="";
            var queuePath = "demoqueue";

            using (var messageSender = new MessageSenderService(connectionString, queuePath))
            {
                //Send messages 
                for (var x = 0; x < 10; x++) messageSender.SendMessage($"Message: {x}");
            }
        }
    }
}