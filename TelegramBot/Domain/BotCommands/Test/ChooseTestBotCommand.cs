using TelegramBot.BotCommandSteps.Test.TestProcessing;

namespace TelegramBot.BotCommands.Test
{
    public class ChooseTestBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Choose test",
            ["ru"] = "Выбрать тест"
        };

        public string Description => "showing tests and will start choose test";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.TestListIsEmpty)
                    , context.GetLocalizedString(LocalizationConstants.UseToCreateNewTest)
                    , new CreateTestShowBotCommand().Keys[context.Client.GetLanguage()]);
            }
            
            context.Client.CommandStepsQueue.Add(new ChooseTestBotCommandStep());
            return context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.ChooseTestThatYouWillCheck)
                , context.Client.TestManager.GetAllTestNames());
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.Client.TestManager.IsTestCollectionEmpty() is false;
        }
    }
}