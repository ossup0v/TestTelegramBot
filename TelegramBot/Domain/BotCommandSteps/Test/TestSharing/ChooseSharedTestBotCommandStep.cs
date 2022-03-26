using TelegramBot.BotCommands;
using TelegramBot.BotCommands.Test;
using TelegramBot.BotCommandSteps.Test.TestProcessing;
using TelegramBot.Database;
using TelegramBot.Database.DTO;
using TelegramBot.Test;

namespace TelegramBot.BotCommandSteps.Test.TestSharing
{
    public sealed class ChooseSharedTestBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            Guid id = default;

            if (Guid.TryParse(context.RawInput, out id) is false)
            {
                await CantFindTest(context);
                return;
            }

            var targetTest = await TestDatabaseMongo.Instance.GetTestById(id);

            if (targetTest is null)
            {
                await CantFindTest(context);
                return;
            }

            await context.SendMessage(context.GetLocalizedString(LocalizationConstants.FoundTestWithNameAndId, context.RawInput), targetTest.Id.ToString());

            TestCollection test = MyMapper.Map<TestCollectionData, TestCollection>(targetTest);

            context.Client.TestManager.CurrentTest = test;

            context.RemoveCommandStep(this);

            await context.SendCallbacks(context.Client.TestManager.CurrentTest.GetQuestion()
                , context.GetTestStepButtons());

            context.AddCommandStep(new ChooseTestAnswerBotCommandStep());
        }

        private Task CantFindTest(CommandExecutionContext context)
        {
            context.RemoveCommandStep(this);
            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.CantFindTestWithSharedId, context.RawInput));
        }
    }
}
