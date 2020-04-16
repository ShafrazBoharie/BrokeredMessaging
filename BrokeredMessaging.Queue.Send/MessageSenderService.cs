using System;
using System.Text;
using Microsoft.Azure.ServiceBus;

namespace BrokeredMessaging.Queue.Send
{
    public class MessageSenderService : IDisposable
    {
        private readonly QueueClient _queueClient;

        public MessageSenderService(string connectionString, string queuePath)
        {
            _queueClient = new QueueClient(connectionString, queuePath);
        }

        public void SendMessage(string messageContent)
        {
            var message = new Message(Encoding.UTF8.GetBytes(messageContent));
            _queueClient.SendAsync(message).Wait();
            Console.WriteLine($"Sent :{messageContent}");
        }

        public void Dispose()
        {
            _queueClient.CloseAsync();
            Console.WriteLine("Sent messages...");
            Console.ReadLine();
        }
    }
}