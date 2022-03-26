using TelegramBot.BotCommandSteps.Test.TestCreating;

namespace TelegramBot.BotCommands.Test
{
    public class CreateTestShowBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Create test",
            ["ru"] = "Создать тест",
        };
        
        public string Description => "will create new test";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.Client.CommandStepsQueue.Add(new CreateNewTestBotCommandStep());
            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.TypeNameOfNewTest));
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}