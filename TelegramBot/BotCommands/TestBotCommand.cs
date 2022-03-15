namespace TelegramBot.BotCommands
{
    internal class TestBotCommand : IBotCommand
    {
        public string Key => "/test";

        public string Description => "for test something";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.SendCallbacks("Choose answer", "answ1", "answ2", "answ3");
        }
    }
}
