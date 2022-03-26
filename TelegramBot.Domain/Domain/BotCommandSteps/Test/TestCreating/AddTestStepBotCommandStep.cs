using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class AddTestStepBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.RawInput == context.GetLocalizedString(LocalizationConstants.Done))
            {
                context.RemoveCommandStep(this);
                var test = context.Client.TestManager.EndCreateNewTest();
                return context.SendAvailableCommands(context.GetLocalizedString(LocalizationConstants.TestWithNameWasCreated, test.Name), test.Id.ToString());
            }

            var questionAndAnswer = context.RawInput.Split('-');
            if (questionAndAnswer.Length is not 2)
            {
                return context.SendMessage(context.GetLocalizedString(LocalizationConstants.TestStepIsNotCorrect, context.RawInput)
                    , context.GetLocalizedString(LocalizationConstants.LooksLikeQuestionAnswer)
                    , context.GetLocalizedString(LocalizationConstants.TryAgain));
            }

            var question = questionAndAnswer[0].Trim();
            var answer = questionAndAnswer[1].Trim();

            context.Client.TestManager.AddNewTestStep(question, answer);
            return context.RemoveMessage();
        }
    }
}