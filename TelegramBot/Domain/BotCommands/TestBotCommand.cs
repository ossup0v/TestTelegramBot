namespace TelegramBot.BotCommands
{
    [NotAvailableCommand]
    public sealed class TestBotCommand : IBotCommand
    {
        public Dictionary<string, string> Keys => new Dictionary<string, string>
        {
            ["en"] = "Test",
            ["ru"] = "Тест",
        };

        public string Description => "for test something";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.SendMessage(context.GetLocalizedString(LocalizationConstants.NameOfTestAlreadyExists, "ewqewq"));
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
