using TelegramBot.BotCommandSteps.Test.TestCreating;

namespace TelegramBot.BotCommands.Test
{
    public class DeleteTestBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Delete test",
            ["ru"] = "Удалить тест",
        };
        
        public string Description => "will delete exists test";

        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                await context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.TestListIsEmpty)
                    , context.GetLocalizedString(LocalizationConstants.UseToCreateNewTest)
                    , new CreateTestShowBotCommand().Keys[context.Client.GetLanguage()]);
                return;
            }

            context.Client.CommandStepsQueue.Add(new DeleteTestBotCommandStep());

            await context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.ChooseTestThatYouWillDelete)
                , context.Client.TestManager.GetAllTestNames());

             await context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.ToCancelType)
                 , TestConstants.CancelTestDelete);
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.Client.TestManager.IsTestCollectionEmpty() is false;
        }
    }
}