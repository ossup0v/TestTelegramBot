using TelegramBot.BotCommands;
using TelegramBot.Domain.Domain.BotCommands.Common;
using TelegramBot.Domain.Domain.BotCommandSteps.ChessGame;

namespace TelegramBot.Domain.Domain.BotCommands.ChessGame
{
    public sealed class StartPlayChessGameOfflineBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys =>
            new Dictionary<string, string>
            {
                ["ru"] = "Шахматы Offline",
                ["en"] = "Chess Offline"
            };
        
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new ChessGamePlayBotCommandStep(true));
            return context.SendReply("Шахматы", "Начать");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
