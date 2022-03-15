using TelegramBot.BotCommandSteps.Test.TestEditing;

namespace TelegramBot.BotCommands.Test
{
    [NotAvailableCommand]
    public sealed class EditTestBotCommand : IBotCommand
    {
        public string Key => "Edit test";

        public string Description => "you can choose test that you will edit";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendCallbacks($"Your test list is empty \nUse to create new test", new CreateTestShowBotCommand().Key);
            }

            context.AddCommandStep(new ChooseTestToEditBotCommandStep());

            return context.SendCallbacks("Choose test to edit", context.Client.TestManager.GetAllTestNames());
        }
    }
}
