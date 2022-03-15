using TelegramBot.BotCommandSteps.Test.TestCreating;

namespace TelegramBot.BotCommands.Test
{
    public class DeleteTestBotCommand : IBotCommand
    {
        public string Key => "Delete test";

        public string Description => "will delete exists test";

        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                await context.SendCallbacks($"Your test list is empty \nUse to create new test", new CreateTestShowBotCommand().Key);
                return;
            }

            context.Client.CommandStepsQueue.Add(new DeleteTestBotCommandStep());

            await context.SendCallbacks($"Pick test that you will delete"
                , context.Client.TestManager.GetAllTestNames());

             await context.SendCallbacks($"To cancel deleting type"
                 , TestConstants.CancelTestDelete);
        }
    }
}