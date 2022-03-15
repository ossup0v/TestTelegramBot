using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.BotClient;
using TelegramBot.BotCommandSteps;

namespace TelegramBot.BotCommands
{
    public sealed class CommandExecutionContext
    {
        public Client? Client;
        public ITelegramBotClient? BotClient;
        public Update? Update;
        public string? RawInput;
        private readonly ILogger _logger;

        private long _chatId => Update?.Message?.Chat?.Id ?? Update.CallbackQuery.Message.Chat.Id;
        private string _userName => Update?.Message?.Chat?.Username ?? Update?.CallbackQuery?.Message?.Chat?.Username ?? "John Doe";

        public CommandExecutionContext(ILogger logger)
        {
            _logger = logger;
        }

        public CommandExecutionContext Fill(string rawInput, Client? client, ITelegramBotClient? botClient, Update update)
        {
            RawInput = rawInput;
            Client = client;
            BotClient = botClient;
            Update = update;

            return this;
        }

        public Task SendMessage(string message)
        {
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:\n" + message);
            return BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    disableNotification: true,
                    text: message);
        }

        public Task SendMessage(params string[] message)
        {
            var text = string.Join("\n", message);
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:\n" + text);
            return BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    disableNotification: true,
                    text: text);
        }

        public Task SendCallbacks(string text, params string[] callbacks)
        {
            text = text.Replace("-", "\\-");
            var buttonCallbackData = callbacks.Select(x => InlineKeyboardButton.WithCallbackData(x.Replace("!", "\\!").Replace("-", "\\-"))).ToArray();
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:\n" + text + "\nand callbacks \n" + String.Join("\n", callbacks));

            return BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    text: text,
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyMarkup: new InlineKeyboardMarkup(buttonCallbackData)
            );
        }

        public Task SendCallbacks(string text, params (string, string)[] callbacks)
        {
            var buttonCallbackData = callbacks.Select(x => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(x.Item1), InlineKeyboardButton.WithCallbackData(x.Item2, x.Item1) });
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:\n" + text + "\nand callbacks\n" + String.Join("\n", callbacks.Select(x=>x.Item1 + " " + x.Item2)));

            return BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    text: text,
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    //replyToMessageId: Update.Message.MessageId,
                    replyMarkup: new InlineKeyboardMarkup(buttonCallbackData)
            );
        }

        public async Task UpdateCallback(string text, params string[] callbacks)
        {
            //ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            //{
            //    new KeyboardButton[] { "One", "Two" },
            //    new KeyboardButton[] { "Three", "Four", "ЭЭ" },
            //})
            //{
            //    ResizeKeyboard = true
            //};
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "Help me", "Call me ☎️" },
            })
            {
                ResizeKeyboard = true
            };

            //var chatId = (ChatId)Update.Message.Chat;

            //return Task.CompletedTask;

            await BotClient.EditMessageTextAsync(Update?.ChosenInlineResult?.InlineMessageId, text);
            //Update.EditedMessage.Text = text;
            //Update.CallbackQuery.Message.Text = text;

            //return BotClient.SendTextMessageAsync(
            //        chatId: _chatId,
            //        text: "Removing keyboard",
            //        replyMarkup: replyKeyboardMarkup);
        }

        public void RemoveCommandStep(IBotCommandStep step)
        {
            Client.CommandStepsQueue.Remove(step);
        }

        public void AddCommandStep(IBotCommandStep step)
        {
            Client.CommandStepsQueue.Add(step);
        }
    }
}