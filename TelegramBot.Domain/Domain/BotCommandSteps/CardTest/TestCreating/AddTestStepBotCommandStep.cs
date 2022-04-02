using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class AddTestStepBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.RawInput == context.GetLocalizedString(LocalizationConstants.Done))
            {
                context.RemoveCommandStep(this);
                var test = context.Client.TestManager.EndCreateNewTest();
                await context.SendAvailableCommands(context.GetLocalizedString(LocalizationConstants.TestWithNameWasCreated, test.Name), test.Id.ToString());
                return;
            }

            var questionAndAnswer = context.RawInput.Split('-');
            if (questionAndAnswer.Length is not 2)
            {
                await context.SendMessage(context.GetLocalizedString(LocalizationConstants.TestStepIsNotCorrect, context.RawInput)
                    , context.GetLocalizedString(LocalizationConstants.LooksLikeQuestionAnswer)
                    , context.GetLocalizedString(LocalizationConstants.TryAgain));
                return;
            }
            
            await context.RemoveMessage();

            await context.SendReply(context.GetLocalizedString(LocalizationConstants.TypeTestSteps) + Environment.NewLine +
                   context.GetLocalizedString(LocalizationConstants.LooksLikeQuestionAnswer) + Environment.NewLine +
                   context.GetLocalizedString(LocalizationConstants.TypeExitForFinishAddingTestSteps)
                 , context.GetLocalizedString(LocalizationConstants.Done));

            var question = questionAndAnswer[0].Trim();
            var answer = questionAndAnswer[1].Trim();

            context.Client.TestManager.AddNewTestStep(question, answer);
        }
    }
}