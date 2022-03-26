using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class DeleteTestBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            var testToDelete = context.RawInput;

            if (testToDelete == TestConstants.CancelTestDelete)
            {
                context.RemoveCommandStep(this);
                return context.SendAvailableCommands(context.GetLocalizedString(LocalizationConstants.DeletionTestIsCancaled));
            }

            if (!context.Client.TestManager.Tests.ContainsKey(testToDelete))
            {
                return context.SendReply(
                    context.GetLocalizedString(LocalizationConstants.CantFindTestWithName, testToDelete) + Environment.NewLine + context.GetLocalizedString(LocalizationConstants.ChooseOneOfThisTest)
                    , context.Client.TestManager.GetAllTestNames());
            }

            context.Client.TestManager.RemoveTest(testToDelete);
            context.RemoveCommandStep(this);
            return context.SendAvailableCommands(context.GetLocalizedString(LocalizationConstants.TestSuccessfullyDeleted, testToDelete));
        }
    }
}