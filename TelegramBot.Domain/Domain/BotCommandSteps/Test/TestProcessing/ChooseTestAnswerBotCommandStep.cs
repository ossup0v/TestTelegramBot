using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;

namespace TelegramBot.BotCommandSteps.Test.TestProcessing
{
    public class ChooseTestAnswerBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            var currentTest = context.Client.TestManager.CurrentTest;
            if (context.RawInput == context.GetLocalizedString(LocalizationConstants.ShowCorrectAnswer))
            {
                await context.SendCallbacks(currentTest.GetAnswer(), context.GetTestStepButtons());
            }
            else if (context.RawInput == context.GetLocalizedString(LocalizationConstants.IKnow) || context.RawInput == context.GetLocalizedString(LocalizationConstants.IDontKnow))
            {
                currentTest.ApplyAnswer(context.RawInput == context.GetLocalizedString(LocalizationConstants.IKnow));

                if (currentTest.IsDone)
                {
                    context.RemoveCommandStep(this);
                    await SendTestIsDone(context);
                    currentTest.Reset();
                }
                else
                {
                    await SendNextTestStep(context);
                }
            }
            else
            {
                await context.SendCallbacks(context.GetLocalizedString(LocalizationConstants.CantUnderstandYouPeekOneOfThisCommands), context.GetTestStepButtons());
            }
        }

        private Task SendNextTestStep(CommandExecutionContext context)
        {
            return context.SendCallbacks(GetAnswerAndQuestionCount(context) + Environment.NewLine + context.Client.TestManager.CurrentTest.GetQuestion()
                , context.GetTestStepButtons());
        }

        private Task SendTestIsDone(CommandExecutionContext context)
        {
            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.TestIsDone)
                , GetAnswerAndQuestionCount(context));
        }

        private string GetAnswerAndQuestionCount(CommandExecutionContext context)
        {
            return $"{context.Client.TestManager.CurrentTest.GetCorrectAnswerCount()} / {context.Client.TestManager.CurrentTest.GetAllQuestionCount()}";
        }
    }
}
