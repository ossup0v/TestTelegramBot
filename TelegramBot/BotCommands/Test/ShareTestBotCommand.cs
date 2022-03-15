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
        public string Key => "Share test";

        public string Description => "Will show id for test share";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.IsTestCollectionEmpty())
            {
                return context.SendCallbacks($"Your test list is empty \nUse to create new test", new CreateTestShowBotCommand().Key);
            }

            context.Client.CommandStepsQueue.Add(new ShowTestIdToShareBotCommandStep());
            return context.SendCallbacks("Choose test that you will share", context.Client.TestManager.GetAllTestNames());
        }
    }
}
