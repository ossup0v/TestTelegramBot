using TelegramBot.BotCommands;

namespace TelegramBot.Domain.Domain.BotCommands.Common
{
    public interface IBotCommand
    {
        Dictionary<string, string> Keys { get; }

        bool IsCanExecute(CommandExecutionContext context);
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
