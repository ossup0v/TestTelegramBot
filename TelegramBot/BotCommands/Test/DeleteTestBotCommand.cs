using TelegramBot.BotCommandSteps.Test.TestCreating;

namespace TelegramBot.BotCommands.Test
{
    public class DeleteTestBotCommand : IBotCommand
    {
        public string Key => "/deletetest";

        public string Description => "will delete exists test";

        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                await context.SendMessage($"Your test list is empty!", $"To create test type command {new CreateTestShowBotCommand().Key}");
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