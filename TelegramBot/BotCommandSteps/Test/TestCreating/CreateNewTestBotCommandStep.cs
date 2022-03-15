using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class CreateNewTestBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (!context.Client.TestManager.StartCreateNewTest(context.RawInput))
            {
                await context.SendMessage($"Name of test '{context.RawInput}' already exists, type something different!");
                return;
            }

            context.RemoveCommandStep(this);
            context.AddCommandStep(new AddTestStepBotCommandStep());
            await context.SendMessage("Type test step"
                , "Like question - answer"
                , "Type '/exit' for finish adding test steps");
        }
    }
}