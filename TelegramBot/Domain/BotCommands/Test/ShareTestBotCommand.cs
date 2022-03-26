using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BotCommandSteps.Test.TestSharing;

namespace TelegramBot.BotCommands.Test
{
    public sealed class ShareTestBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Share test",
            ["ru"] = "Поделиться тестом",
        };

        public string Description => "Will show id for test share";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.TestListIsEmpty)
                    , context.GetLocalizedString(LocalizationConstants.UseToCreateNewTest)
                    , new CreateTestShowBotCommand().Keys[context.Client.GetLanguage()]);
            }

            context.Client.CommandStepsQueue.Add(new ShowTestIdToShareBotCommandStep());
            return context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.ChooseTestThatYouWillShare)
                , context.Client.TestManager.GetAllTestNames());
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.Client.TestManager.IsTestCollectionEmpty() is false;
        }
    }
}
