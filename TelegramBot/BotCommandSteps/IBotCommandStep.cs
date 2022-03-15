using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps
{
    public interface IBotCommandStep
    {
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
