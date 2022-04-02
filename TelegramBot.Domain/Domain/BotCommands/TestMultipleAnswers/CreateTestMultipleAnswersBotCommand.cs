using TelegramBot.BotCommands;
using TelegramBot.BotCommandSteps;
using TelegramBot.Domain.Domain.BotCommands.Common;

namespace TelegramBot.Domain.Domain.BotCommands.TestMultipleAnswers
{
    [NotAvailableCommand]
    public sealed class CreateTestMultipleAnswersBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys =>
            new Dictionary<string, string>
            {
                ["en"] = "Create multy Test",
                ["ru"] = "Создать мульти тест"
            };

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.Client.CommandStepsQueue.Add(new CreateTestMultipleAnswersBotCommandStep());
            return context.SendReply(context.GetLocalizedString(LocalizationConstants.TypeNameOfNewTest));
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }

    public sealed class CreateTestMultipleAnswersBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (!context.Client.TestManager.StartCreateNewTest(context.RawInput))
            {
                return context.SendMessage(context.GetLocalizedString(LocalizationConstants.NameOfTestAlreadyExists, context.RawInput));
            }

            context.RemoveCommandStep(this);
            context.AddCommandStep(new AddTestMultipleAnswersQuestionBotCommandStep());
            return context.SendReply(context.GetLocalizedString(LocalizationConstants.TypeTestSteps) + Environment.NewLine +
                 context.GetLocalizedString(LocalizationConstants.TypeExitForFinishAddingTestSteps)
                 , context.GetLocalizedString(LocalizationConstants.Done));
        }
    }

    public sealed class AddTestMultipleAnswersQuestionBotCommandStep : IBotCommandStep
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
