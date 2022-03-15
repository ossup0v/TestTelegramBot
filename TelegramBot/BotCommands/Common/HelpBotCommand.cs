using TelegramBot.BotCommands.Attributes;

namespace TelegramBot.BotCommands
{
    [NotAvailableCommand]
    public class HelpBotCommand : IBotCommand
    {
        public string Key => "/help";

        public string Description => "will show you all available commands";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.SendCallbacks("All available commands is", AllCommandsHelper.CommandsWithDescription);
        }
    }
}