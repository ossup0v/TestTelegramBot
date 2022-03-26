using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.BotCommands;
using TelegramBot.Domain.Domain.BotClient;

namespace TelegramBot.BotNetwork
{
    public sealed class BotMessageHandler
    {
        private readonly Dictionary<long, Client> _clients;
        private readonly ILogger _logger;

        public BotMessageHandler(ILogger logger)
        {
            _clients = new Dictionary<long, Client>();
            _logger = logger;
        }

        public Task HandleMessage(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message)
            {
                return ProcessMessage(botClient, update, token);
            }

            if (update.Type == UpdateType.CallbackQuery)
            { 
                return ProcessCallbackQuery(botClient, update, token);
            }

            _logger.LogError($"Can't process input with type {update.Type} and with message {update.Message?.Text}");

            return Task.CompletedTask;
        }

        private Task ProcessMessage(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Type != UpdateType.Message)
                return Task.CompletedTask;
            if (update.Message!.Type != MessageType.Text)
                return Task.CompletedTask;

            var userId = update.Message.From?.Id ?? 0;
            var messageText = update.Message.Text;
            var chat = update.Message.Chat;

            if (!_clients.ContainsKey(userId))
                _clients.Add(userId, new Client(userId, _logger, AllCommandsHelper.BotCommands));

            _logger.LogInformation($"Received a '{messageText}' message from user id: {userId}, user name: {chat.Username}");

            return _clients[userId].ProccessMessageInput(messageText, botClient, update);
        }

        private Task ProcessCallbackQuery(ITelegramBotClient botClient, Update update, CancellationToken token) 
        {
            if (update.Type != UpdateType.CallbackQuery)
                return Task.CompletedTask;

            var userId = update.CallbackQuery.From?.Id ?? 0;
            var data = update.CallbackQuery.Data;
            var chat = update.CallbackQuery.Message.Chat;

            if (!_clients.ContainsKey(userId))
                _clients.Add(userId, new Client(userId, _logger, AllCommandsHelper.BotCommands));

            _logger.LogInformation($"Received '{data}' data from {userId}, user name: {chat.Username}");

            return _clients[userId].ProccessCallbackQueryInput(data, botClient, update);
        }

        public Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(errorMessage);

            return Task.CompletedTask;
        }
    }
}
