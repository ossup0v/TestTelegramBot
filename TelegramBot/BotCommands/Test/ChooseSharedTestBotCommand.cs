using TelegramBot.BotCommandSteps.Test.TestSharing;

namespace TelegramBot.BotCommands.Test
{
    public sealed class ChooseSharedTestBotCommand : IBotCommand
    {
        public string Key => "Choose shared test";

        public string Description => "choose test from other user by unic id";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new ChooseSharedTestBotCommandStep());

            return context.SendMessage("Type here test shared id", $"Looks like {Guid.NewGuid()}");
        }
    }
}
