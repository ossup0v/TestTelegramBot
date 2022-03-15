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
                return context.SendCallbacks($"Can't find test with name {testToShareName}\nChoose one of this tests", context.Client.TestManager.GetAllTestNames());
            }

            var test = context.Client.TestManager.Tests[testToShareName];

            context.RemoveCommandStep(this);
            return context.SendMessage($"Test id is", test.Id.ToString());
        }
    }
}
