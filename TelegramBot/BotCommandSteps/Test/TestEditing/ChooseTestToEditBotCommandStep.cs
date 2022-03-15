using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestEditing
{
    public sealed class ChooseTestToEditBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            var testToEdit = context.RawInput;

            if (context.Client.TestManager.IsContainsTest(testToEdit) is false)
            {
                return context.SendCallbacks($"Can't find test with name {testToEdit}\nChoose one of this tests", context.Client.TestManager.GetAllTestNames());
            }

            context.Client.TestManager.ChooseCurrentTest(testToEdit);

            return Task.CompletedTask;
        }
    }
}
