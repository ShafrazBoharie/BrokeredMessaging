using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace BrokeredMessaging.Chat.Services
{
    public class ChatApplication
    {
        private readonly ManagementClient manager;
        private SubscriptionClient subscriptionClient;
        private TopicClient topicClient;


        public ChatApplication(string connectionString, string topicPath, string username)
        {
            manager = new ManagementClient(connectionString);
            CreateChatApplication(connectionString, topicPath, username);
        }


        private void CreateChatApplication(string connectionString, string topicPath, string username)
        {
            // #1 Create Topic if it does not exist 
            if (!manager.TopicExistsAsync(topicPath).Result)
                manager.CreateTopicAsync(topicPath).Wait();

            // #2 Create a subscription for the user 
            if (!manager.SubscriptionExistsAsync(topicPath, username).Result)
            {
                var description = new SubscriptionDescription(topicPath, username)
                {
                    AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
                };
                manager.CreateSubscriptionAsync(description).Wait();
            }

            // #3 Create Clients 
            topicClient = new TopicClient(connectionString, topicPath);
            subscriptionClient = new SubscriptionClient(connectionString, topicPath, username);

            // #3 Create a Message handler for receiving messages 
            subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, ExceptionReceivedHandler);

            // #4 Send a message to notify you have entered
            var helloMessage = new Message(Encoding.UTF8.GetBytes("Has entered the room"));
            helloMessage.Label = username;
            topicClient.SendAsync(helloMessage).Wait();

            while (true)
            {
                var text = Console.ReadLine();
                if (text.Equals("exit")) break;

                // send a chat message 
                var chatMessage = new Message(Encoding.UTF8.GetBytes(text));
                chatMessage.Label = username;
                topicClient.SendAsync(chatMessage).Wait();
            }

            // Send a message to notify you have left the room
            var goodbyeMessage = new Message(Encoding.UTF8.GetBytes("Has left the building"));
            goodbyeMessage.Label = username;
            topicClient.SendAsync(goodbyeMessage).Wait();

            //Close the clients
            topicClient.CloseAsync().Wait();
            subscriptionClient.CloseAsync().Wait();
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            //Deserializes the message body
            var text = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"{message.Label}> {text}");
        }
    }
}