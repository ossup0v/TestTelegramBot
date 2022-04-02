using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BotCommands;
using TelegramBot.Domain.Domain.BotCommands.Common;
using TelegramBot.Domain.Domain.BotCommandSteps.OXPlay;

namespace TelegramBot.Domain.Domain.BotCommands.OXPlay
{
    public sealed class ConnectToOXGameBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys =>
            new Dictionary<string, string>
            {
                ["ru"] = "Подключиться к игре",
                ["en"] = "Connect to game",
            };

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new PlayOXBotCommandStep());
            return context.SendReply("Напишите Id игры");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
