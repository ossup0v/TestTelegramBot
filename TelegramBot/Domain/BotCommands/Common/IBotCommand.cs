namespace TelegramBot.BotCommands
{
    public interface IBotCommand
    {
        Dictionary<string, string> Keys { get; } //showallusers/показать всех пользователей
        string Description { get; } // send to user all other users/ покажет всех пользователей

        bool IsCanExecute(CommandExecutionContext context);
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
