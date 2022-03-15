using TelegramBot.BotCommandSteps.Test.TestCreating;

namespace TelegramBot.BotCommands.Test
{
    public class CreateTestShowBotCommand : IBotCommand
    {
        public string Key => "Create test";

        public string Description => "will create new test";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.Client.CommandStepsQueue.Add(new CreateNewTestBotCommandStep());
            return context.SendMessage("Type name of new test!");
        }
    }
}