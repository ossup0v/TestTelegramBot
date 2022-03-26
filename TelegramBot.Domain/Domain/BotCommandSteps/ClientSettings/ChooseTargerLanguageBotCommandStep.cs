using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.ClientSettings
{
    public sealed class ChooseTargerLanguageBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (LocalizationConstants.AvailableLanguages.Contains(context.RawInput) is false)
            {
                return context.SendCallbacksInCulomn(context.GetLocalizedString(LocalizationConstants.ShowAllAvailableLanguagesStr), LocalizationConstants.AvailableLanguages);
            }

            context.Client.ChangeTargetLanguage(context.RawInput);
            context.RemoveCommandStep(this);
            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.SetTargetLanguageSuccess));
        }
    }
}
