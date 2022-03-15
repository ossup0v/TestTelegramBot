using AutoMapper;
using Microsoft.Extensions.Logging;

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

            var botManager = new BotManager(loggerFactory.CreateLogger<BotManager>());

            botManager.Start();

            Console.ReadLine();

            botManager.Stop();
        }
    }
}
