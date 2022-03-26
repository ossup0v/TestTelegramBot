using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class CreateNewTestBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (!context.Client.TestManager.StartCreateNewTest(context.RawInput))
            {
                return context.SendMessage(context.GetLocalizedString(LocalizationConstants.NameOfTestAlreadyExists, context.RawInput));
            }

            context.RemoveCommandStep(this);
            context.AddCommandStep(new AddTestStepBotCommandStep());
            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.TypeTestSteps)
                , context.GetLocalizedString(LocalizationConstants.LooksLikeQuestionAnswer)
                , context.GetLocalizedString(LocalizationConstants.TypeExitForFinishAddingTestSteps));
        }
    }
}