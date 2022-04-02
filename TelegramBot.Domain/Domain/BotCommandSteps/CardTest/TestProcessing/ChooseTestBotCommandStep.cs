using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;

namespace TelegramBot.BotCommandSteps.Test.TestProcessing
{
    public class ChooseTestBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.Client.TestManager.TryChooseTest(context.RawInput))
            {
                context.Client.CommandStepsQueue.Remove(this);

                if (context.Client.TestManager.CurrentTest.GetTestSteps().Count == 0)
                {
                    return context.SendMessage($"Test {context.Client.TestManager.CurrentTest.Name} is empty, you already done it !");
                }

                context.AddCommandStep(new ChooseTestAnswerBotCommandStep());
                return context.SendReply(context.Client.TestManager.CurrentTest.GetQuestion(), context.GetTestStepButtons());
            }
            else
            {
                return context.SendReply(
                    context.GetLocalizedString(LocalizationConstants.CantFindTestWithName, context.RawInput) + Environment.NewLine + context.GetLocalizedString(LocalizationConstants.ChooseOneOfThisTest)
                    , context.Client.TestManager.GetAllTestNames());
            }
        }
    }
}
