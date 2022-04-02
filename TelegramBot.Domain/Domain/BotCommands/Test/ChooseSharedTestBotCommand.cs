using TelegramBot.BotCommandSteps.Test.TestSharing;
using TelegramBot.Domain.Domain.BotCommands.Common;
using TelegramBot.Domain.Domain.BotCommandSteps.Test.TestSharing;

namespace TelegramBot.BotCommands.Test
{
    [NotAvailableCommand]

    public sealed class ChooseSharedTestBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Choose shared test",
            ["ru"] = "Ввести id теста",
        };

        public string Description => "choose test from other user by unic id";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new ChooseSharedTestBotCommandStep());

            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.TypeHereTestSharedId)
                , context.GetLocalizedString(LocalizationConstants.LooksLike) + Guid.NewGuid().ToString());
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
