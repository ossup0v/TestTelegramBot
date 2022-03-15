using TelegramBot.BotCommands.Attributes;
using TelegramBot.BotCommandSteps.Test.TestEditing;

namespace TelegramBot.BotCommands.Test
{
    [NotAvailableCommand]
    public sealed class EditTestBotCommand : IBotCommand
    {
        public string Key => "/edittest";

        public string Description => "you can choose test that you will edit";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendMessage($"Your test list is empty!", $"To create test type command {new CreateTestShowBotCommand().Key}");
            }

            context.AddCommandStep(new ChooseTestToEditBotCommandStep());

            return context.SendCallbacks("Choose test to edit", context.Client.TestManager.GetAllTestNames());
        }
    }
}
