using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class DeleteTestBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            var testToDelete = context.RawInput;

            if (testToDelete == TestConstants.CancelTestDelete)
            {
                await context.SendMessage($"Test deleting is successfully cancaled!");
                context.RemoveCommandStep(this);
                return;
            }

            if (!context.Client.TestManager.Tests.ContainsKey(testToDelete))
            {
                await context.SendCallbacks($"Can't find test with name {testToDelete}\nChoose one of this tests", context.Client.TestManager.GetAllTestNames());
                return;
            }

            context.Client.TestManager.RemoveTest(testToDelete);
            context.RemoveCommandStep(this);
            await context.SendMessage($"Test {testToDelete} is successfully deleted!");
        }
    }
}