namespace TelegramBot.BotCommands
{
    [NotAvailableCommand]
    public class HelpBotCommand : IBotCommand
    {
        public string Key => "Help";

        public string Description => "will show you all available commands";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.SendCallbacks("All available commands is", AllCommandsHelper.CommandsWithDescription);
        }
    }
}