using TelegramBot.BotCommandSteps.ClientSettings;
using TelegramBot.Domain.Domain.BotCommands.Common;

namespace TelegramBot.BotCommands
{
    [NotAvailableCommand]
    public sealed class SetLanguageBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Set language",
            ["ru"] = "Выбрать язык",
        };

        public string Description => "ewq";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new ChooseTargerLanguageBotCommandStep());
            return context.SendReplyInColmn(context.GetLocalizedString(LocalizationConstants.ShowAllAvailableLanguagesStr), LocalizationConstants.AvailableLanguages);
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
