using TelegramBot.BotCommands;
using TelegramBot.Domain.Domain.BotCommands.Common;
using TelegramBot.Domain.Domain.BotCommandSteps;

namespace TelegramBot.Domain.Domain.BotCommands
{
    public sealed class ConnectToGameBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys =>
            new Dictionary<string, string>
            {
                ["ru"] = "Подключиться к игре",
                ["en"] = "Connect to game",
            };

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new ConnectToGameBotCommandStep());
            return context.SendReply("Напишите Id игры");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
