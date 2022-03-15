namespace TelegramBot.BotCommands
{
    //[NotAvailableCommand]
    public sealed class TestBotCommand : IBotCommand
    {
        public string Key => "Test";

        public string Description => "for test something";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.UpdateCallback("Choose answer", "answ1", "answ2", "answ3");
        }
    }
}
