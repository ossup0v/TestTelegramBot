namespace TelegramBot.BotCommands
{
    public interface IBotCommand
    {
        Dictionary<string, string> Keys { get; } //showallusers/показать всех пользователей

        bool IsCanExecute(CommandExecutionContext context);
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
