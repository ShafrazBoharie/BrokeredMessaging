using System;
using BrokeredMessaging.Chat.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BrokeredMessaging.Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString ="";
            string topicPath = "chatTopic";
            InitiateChatApplication(connectionString,topicPath);
        }

        private static void InitiateChatApplication(string connectionString, string topicPath)
        {
            Console.WriteLine("EnterName:");
            var username = Console.ReadLine();
            var chatApplication= new ChatApplication(connectionString,topicPath,username);
        }
    } 
}
