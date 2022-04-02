using TelegramBot.BotCommands;
using TelegramBot.Domain.Domain.BotCommands.Common;
using TelegramBot.Domain.Domain.BotCommandSteps.OXPlay;

namespace TelegramBot.Domain.Domain.BotCommands.OXPlay
{
    public sealed class StartPlayOXBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys =>
            new Dictionary<string, string>
            {
                ["ru"] = "Крестики нолики",
                ["en"] = "OX game"
            };

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new PlayOXBotCommandStep());
            return context.SendReply("OX GAME", 3, "-", "-", "-",
                                                       "-", "-", "-",
                                                       "-", "-", "-");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
