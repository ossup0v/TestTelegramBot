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
            return context.SendReply("test", "1","2","3");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
