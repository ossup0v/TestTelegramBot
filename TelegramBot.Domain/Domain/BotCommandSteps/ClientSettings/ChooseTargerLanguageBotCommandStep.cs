using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.ClientSettings
{
    public sealed class ChooseTargerLanguageBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (LocalizationConstants.AvailableLanguages.Contains(context.RawInput) is false)
            {
                return context.SendReplyInColmn(context.GetLocalizedString(LocalizationConstants.ShowAllAvailableLanguagesStr), LocalizationConstants.AvailableLanguages);
            }

            context.Client.ChangeTargetLanguage(context.RawInput);
            context.RemoveCommandStep(this);
            return context.SendAvailableCommands(context.GetLocalizedString(LocalizationConstants.SetTargetLanguageSuccess));
        }
    }
}
