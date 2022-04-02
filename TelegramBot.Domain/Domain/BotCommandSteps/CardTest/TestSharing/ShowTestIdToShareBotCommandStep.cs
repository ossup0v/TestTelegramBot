using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestSharing
{
    public sealed class ShowTestIdToShareBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            var testToShareName = context.RawInput;

            if (context.Client.TestManager.IsContainsTest(testToShareName) is false)
            {
                return context.SendCallbacks(
                    context.GetLocalizedString(LocalizationConstants.CantFindTestWithName, testToShareName) + Environment.NewLine + context.GetLocalizedString(LocalizationConstants.ChooseOneOfThisTest)
                    , context.Client.TestManager.GetAllTestNames());
            }

            var test = context.Client.TestManager.Tests[testToShareName];

            context.RemoveCommandStep(this);
            return context.SendAvailableCommands(context.GetLocalizedString(LocalizationConstants.TestIdIs), $"`{test.Id}`");
        }
    }
}
