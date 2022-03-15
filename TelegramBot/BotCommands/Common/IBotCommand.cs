namespace TelegramBot.BotCommands
{
    public interface IBotCommand
    {
        string Key { get; } //showallusers
        string Description { get; } // send to user all other users
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
