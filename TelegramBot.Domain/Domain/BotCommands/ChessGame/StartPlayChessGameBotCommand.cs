using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BotCommands;
using TelegramBot.Domain.Domain.BotCommands.Common;
using TelegramBot.Domain.Domain.BotCommandSteps.ChessGame;

namespace TelegramBot.Domain.Domain.BotCommands.ChessGame
{
    public sealed class StartPlayChessGameBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys =>
            new Dictionary<string, string>
            {
                ["ru"] = "Шахматы",
                ["en"] = "Chess"
            };
        
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new ChessGamePlayBotCommandStep());
            return context.SendReply("Шахматы", "Начать");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
