using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace BrokeredMessaging.Queue.Receive
{
    public class MessageReceiverService : IDisposable
    {
        private readonly QueueClient _queueClient;

        public MessageReceiverService(string connectionString, string queuePath)
        {
            _queueClient = new QueueClient(connectionString, queuePath);

            // Create a message handler to receive messages
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, ExceptionReceivedHandler);
        }

        public void Dispose()
        {
            // Close the client
            _queueClient.CloseAsync().Wait();
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Deserialize the message body.
            var text = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Received: {text}");
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }
    }
}