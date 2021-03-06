using TelegramBot.Domain.Domain.BotCommands.Common;

namespace TelegramBot.BotCommands
{
    [NotAvailableCommand]
    public class HelpBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Help",
            ["ru"] = "Помощь",
        };

        public string Description => "will show you all available commands";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.SendReplyInColmn(context.GetLocalizedString(LocalizationConstants.AllAvailableCommandsIs), AllCommandsHelper.GetCommandKeysToShow(context));
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return false;
        }
    }
}