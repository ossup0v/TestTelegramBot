using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.BotCommandSteps;
using TelegramBot.Domain.Domain.BotClient;

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
        private long _userId => Client.UserId;
        private int _userMessageId => Update?.Message?.MessageId ?? Update?.CallbackQuery?.Message?.MessageId ?? 0;
        private int _lastSendedMessageId;

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

        public Task SendMessage(params string[] message)
        {
            return SendReply(String.Join(Environment.NewLine, message));
        }

        private async Task SendCallbacksInCulomn(string text, params string[] callbacks)
        {
            var buttonCallbackData = callbacks.Select(x => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(x) });
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:" + Environment.NewLine + text + Environment.NewLine + "and callbacks" + Environment.NewLine + String.Join(Environment.NewLine, callbacks));

            var sendedMessage = await BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    text: text,
                    parseMode: ParseMode.Markdown,
                    disableNotification: true,
                    replyMarkup: new InlineKeyboardMarkup(buttonCallbackData)
            );

            _lastSendedMessageId = sendedMessage.MessageId;
        }

        public Task RemoveMessage()
        {
            if (_userMessageId == _lastSendedMessageId)
                return Task.CompletedTask;

            return BotClient.DeleteMessageAsync(
                    chatId: _chatId,
                    messageId: _userMessageId);
        }

        public async Task SendMapInversed(string titleText, int countInRow, params string[] textButtons)
        {
            var temp = new List<List<InlineKeyboardButton>>(textButtons.Length / countInRow);
            var result = new List<List<InlineKeyboardButton>>(textButtons.Length / countInRow);

            var rowCount = 0;
            var columnCount = -1;
            foreach (var button in textButtons)
            {
                if (rowCount % countInRow == 0)
                {
                    temp.Add(new List<InlineKeyboardButton>(countInRow));
                    columnCount++;
                }

                temp[columnCount].Add(InlineKeyboardButton.WithCallbackData(button, $"{rowCount % countInRow}:{columnCount}"));
                rowCount++;
            }

            for (int y = temp.Count - 1, res = 0; y >= 0; y--, res++)
            {
                result.Add(new List<InlineKeyboardButton>());
                result[res].AddRange(temp[y]);
            }

            result.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Сдаться", $"{_userId}:surrender"),
                InlineKeyboardButton.WithCallbackData("Ничья", $"{_userId}:draw")
            });

            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:" + Environment.NewLine + titleText + Environment.NewLine + "and callbacks" + Environment.NewLine + String.Join(Environment.NewLine, textButtons));

            try
            {
                var sendedMessage = await BotClient.EditMessageTextAsync(
                            chatId: _chatId,
                            text: titleText,
                            messageId: _lastSendedMessageId,
                            replyMarkup: new InlineKeyboardMarkup(result),
                            parseMode: ParseMode.Markdown
                            );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SendReply(string titleText, int countInRow, params string[] textButtons)
        {
            var result = new List<List<InlineKeyboardButton>>(textButtons.Length / countInRow);

            var rowCount = 0;
            var columnCount = -1;
            foreach (var button in textButtons)
            {
                if (rowCount % countInRow == 0)
                {
                    result.Add(new List<InlineKeyboardButton>(countInRow));
                    columnCount++;
                }

                result[columnCount].Add(InlineKeyboardButton.WithCallbackData(button, $"{rowCount % countInRow}:{columnCount}"));
                rowCount++;
            }

            result.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Сдаться", $"{_userId}:surrender"),
                InlineKeyboardButton.WithCallbackData("Ничья", $"{_userId}:draw")
            });

            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:" + Environment.NewLine + titleText + Environment.NewLine + "and callbacks" + Environment.NewLine + String.Join(Environment.NewLine, textButtons));

            try
            {
                var sendedMessage = await BotClient.EditMessageTextAsync(
                            chatId: _chatId,
                            text: titleText,
                            messageId: _lastSendedMessageId,
                            replyMarkup: new InlineKeyboardMarkup(result),
                            parseMode: ParseMode.Markdown
                            );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SendReply(string titleText, params string[] textButtons)
        {
            _logger.LogInformation($"User: {_userName} recieved keyboard: \'{titleText}\' with buttons:{Environment.NewLine}{string.Join(Environment.NewLine, textButtons)}");

            try
            {
                await RemoveMessage();
                await UpdateReply(titleText, textButtons);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Bad Request: message is not modified: specified new message content and reply markup are exactly the same as a current content and reply markup of the message")
                {
                    //it's okay
                    return;
                }

                await SendCallbacks(titleText, textButtons);
            }
        }

        public async Task SendReplyInColmn(string titleText, params string[] textButtons)
        {
            _logger.LogInformation($"User: {_userName} recieved keyboard: \'{titleText}\' with buttons:{Environment.NewLine}{string.Join(Environment.NewLine, textButtons)}");


            try
            {
                await RemoveMessage();
                await UpdateReply(titleText, textButtons);
            }
            catch (Exception ex)
            {
                await SendCallbacksInCulomn(titleText, textButtons);
            }
        }

        private Task UpdateReply(string titleText, params string[] textButtons)
        {
            var buttons = textButtons
                .Select(x => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(x) })
                .ToList();

            var inlineKeyboard = new InlineKeyboardMarkup(buttons);

            return BotClient.EditMessageTextAsync(
                        chatId: _chatId,
                        text: titleText,
                        messageId: _lastSendedMessageId,
                        replyMarkup: inlineKeyboard,
                        parseMode: ParseMode.Markdown
                        );
        }

        public async Task SendCallbacks(string titleText, params string[] textButtons)
        {
            titleText = titleText.Replace("-", "\\-");
            var buttonCallbackData = textButtons.Select(x => InlineKeyboardButton.WithCallbackData(x/*.Replace("!", "\\!").Replace("-", "\\-")*/)).ToArray();
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:" + Environment.NewLine + titleText + Environment.NewLine + "and callbacks " + Environment.NewLine + String.Join(Environment.NewLine, textButtons));

            var sendedMessage = await BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    text: titleText,
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyMarkup: new InlineKeyboardMarkup(buttonCallbackData)
            );

            _lastSendedMessageId = sendedMessage.MessageId;
        }

        public void RemoveCommandStep(IBotCommandStep step)
        {
            Client.CommandStepsQueue.Remove(step);
        }

        public void AddCommandStep(IBotCommandStep step)
        {
            Client.CommandStepsQueue.Add(step);
        }

        public string GetLocalizedString(string key)
        {
            return Client.Localizator.GetLocalizedString(key);
        }

        public string GetLocalizedString(string key, params string[] @params)
        {
            return string.Format(Client.Localizator.GetLocalizedString(key), @params);
        }

        public string[] GetTestStepButtons()
        {
            return new string[] { GetLocalizedString(LocalizationConstants.IKnow), GetLocalizedString(LocalizationConstants.ShowCorrectAnswer), GetLocalizedString(LocalizationConstants.IDontKnow) };
        }

        public Task SendAvailableCommands(params string[] titleText)
        {
            return SendReplyInColmn(String.Join(Environment.NewLine, titleText) //+ Environment.NewLine + GetLocalizedString(LocalizationConstants.AllAvailableCommandsIs)
                , AllCommandsHelper.GetCommandKeysToShow(this));
        }
    }
}