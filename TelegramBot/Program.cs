using AutoMapper;
using Microsoft.Extensions.Logging;
using TelegramBot.BotCommands;
using TelegramBot.Database;
using TelegramBot.Domain;

namespace TelegramBot
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddConsole();
            });

            TestDatabaseWrapper.Init(new TestDatabaseMongo());

            var botManager = new BotManager(loggerFactory.CreateLogger<BotManager>());

            botManager.Start();

            Console.ReadLine();

            botManager.Stop();
        }
    }
}
