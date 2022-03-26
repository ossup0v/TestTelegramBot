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

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendReply(context.GetLocalizedString(LocalizationConstants.TestListIsEmpty)
                    , context.GetLocalizedString(LocalizationConstants.UseToCreateNewTest)
                    , new CreateTestShowBotCommand().Keys[context.Client.GetLanguage()]);
            }

            context.Client.CommandStepsQueue.Add(new DeleteTestBotCommandStep());

            return context.SendReply(context.GetLocalizedString(LocalizationConstants.ChooseTestThatYouWillDelete) + Environment.NewLine + context.GetLocalizedString(LocalizationConstants.ToCancelType) + TestConstants.CancelTestDelete
                , context.Client.TestManager.GetAllTestNames(TestConstants.CancelTestDelete));
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.Client.TestManager.IsTestCollectionEmpty() is false;
        }
    }
}