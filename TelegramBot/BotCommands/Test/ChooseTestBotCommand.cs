using TelegramBot.BotCommandSteps.Test.TestProcessing;

namespace TelegramBot.BotCommands.Test
{
    public class ChooseTestBotCommand : IBotCommand
    {
        public string Key => "/choosetest";

        public string Description => "showing tests and will start choose test";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendMessage($"Your test list is empty!", $"To create test type command {new CreateTestShowBotCommand().Key}");
            }
            
            context.Client.CommandStepsQueue.Add(new ChooseTestBotCommandStep());
            return context.SendCallbacks("Choose test that you will check", context.Client.TestManager.GetAllTestNames());
        }
    }
}