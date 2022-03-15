using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.BotClient;
using TelegramBot.BotCommands;

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

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            var chat = update.Message.Chat;

            if (!_clients.ContainsKey(chatId))
                _clients.Add(chatId, new Client(chatId, _logger, AllCommandsHelper.BotCommands));

            _logger.LogInformation($"Received a '{messageText}' message in chat {chatId} an user name: {chat.Username}");

            return _clients[chatId].ProccessMessageInput(messageText, botClient, update);
        }

        private Task ProcessCallbackQuery(ITelegramBotClient botClient, Update update, CancellationToken token) 
        {
            if (update.Type != UpdateType.CallbackQuery)
                return Task.CompletedTask;

            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageText = update.CallbackQuery.Data;
            var chat = update.CallbackQuery.Message.Chat;

            if (!_clients.ContainsKey(chatId))
                _clients.Add(chatId, new Client(chatId, _logger, AllCommandsHelper.BotCommands));

            _logger.LogInformation($"Received a '{messageText}' message in chat {chatId} an user name: {chat.Username}");

            return _clients[chatId].ProccessCallbackQueryInput(messageText, botClient, update);
        }

        public Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);

            return Task.CompletedTask;
        }
    }
}
