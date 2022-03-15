using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;

namespace TelegramBot.BotCommandSteps.Test.TestProcessing
{
    public class ChooseTestBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.TryChooseTest(context.RawInput))
            {
                context.Client.CommandStepsQueue.Remove(this);

                if (context.Client.TestManager.CurrentTest.GetTestSteps().Count == 0)
                { 
                    await context.SendMessage($"Test {context.Client.TestManager.CurrentTest.Name} is empty, you already done it !)");
                    return;
                }

                await context.SendCallbacks(context.Client.TestManager.CurrentTest.GetQuestion(), TestConstants.ChooseAnswerArray);
                context.AddCommandStep(new ChooseTestAnswerBotCommandStep());
            }
            else
            {
                await context.SendCallbacks($"Can't find test with name {context.RawInput}\nChoose one of this tests", context.Client.TestManager.GetAllTestNames());
            }
        }
    }
}
