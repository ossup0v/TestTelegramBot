using TelegramBot.BotCommandSteps.Test.TestEditing;
using TelegramBot.Domain.Domain.BotCommands.Common;

namespace TelegramBot.BotCommands.Test
{
    [NotAvailableCommand]
    public sealed class EditTestBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        { 
            ["ru"] = "Изменить тест",
            ["en"] = "Edit test"
        };

        public string Description => "you can choose test that you will edit";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendReply(context.GetLocalizedString(LocalizationConstants.TestListIsEmpty)
                    , context.GetLocalizedString(LocalizationConstants.UseToCreateNewTest)
                    , new CreateTestShowBotCommand().Keys[context.Client.GetLanguage()]);
            }

            context.AddCommandStep(new ChooseTestToEditBotCommandStep());

            return context.SendReply(context.GetLocalizedString(LocalizationConstants.ChooseTestThatYouWillEdit)
                , context.Client.TestManager.GetAllTestNames());
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.Client.TestManager.IsTestCollectionEmpty() is false;
        }
    }
}
