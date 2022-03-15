using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;

namespace TelegramBot.BotCommandSteps.Test.TestProcessing
{
    public class ChooseTestAnswerBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            var currentTest = context.Client.TestManager.CurrentTest;
            if (context.RawInput == TestConstants.ShowAnswer)
            {
                await context.SendCallbacks(currentTest.GetAnswer()
                    , TestConstants.ChooseAnswerArray);
            }
            else if (context.RawInput == TestConstants.CorrectAnswer || context.RawInput == TestConstants.IncorrecntAnswer)
            {
                currentTest.ApplyAnswer(context.RawInput == TestConstants.CorrectAnswer);

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
                await context.SendCallbacks($"Can't understand you. Peek one of this commands, please!"
                    , TestConstants.ChooseAnswerArray);
            }
        }

        private Task SendNextTestStep(CommandExecutionContext context)
        {
            return context.SendCallbacks(GetAnswerAndQuestionCount(context) + "\n" + context.Client.TestManager.CurrentTest.GetQuestion()
                , TestConstants.ChooseAnswerArray);
        }

        private Task SendTestIsDone(CommandExecutionContext context)
        {
            return context.SendMessage($"Test is done!"
                , GetAnswerAndQuestionCount(context));
        }

        private string GetAnswerAndQuestionCount(CommandExecutionContext context)
        {
            return $"{context.Client.TestManager.CurrentTest.GetCorrectAnswerCount()} / {context.Client.TestManager.CurrentTest.GetAllQuestionCount()}";
        }
    }
}
