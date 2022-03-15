using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.BotCommands;
using TelegramBot.BotCommandSteps;
using TelegramBot.Database;
using TelegramBot.Test;

namespace TelegramBot.BotClient
{
    public sealed class Client
    {
        private readonly CommandExecutionContext _context;
        public TestManager TestManager { get; init; }
        private IReadOnlyDictionary<string, IBotCommand> _availableCommands;
        public List<IBotCommandStep> CommandStepsQueue { get; } = new List<IBotCommandStep>();

        public Client(long chatIdOwner, ILogger logger, IReadOnlyDictionary<string, IBotCommand> availableCommands)
        {
            _context = new CommandExecutionContext(logger);
            _availableCommands = availableCommands;
            TestManager = new TestManager(chatIdOwner,
                TestDatabaseMongo.Instance
                .GetAllClientTests(chatIdOwner)
                .Result
                .Select(x => new TestCollection(x.Name, 
                    x.Id,
                    x.Steps.Select(x => new TestStep 
                    { Answer = x.Answer, Question = x.Question })
                    .ToList()))
                .ToList());
        }

        public Task ProccessMessageInput(string input, ITelegramBotClient botClient, Update update)
        {
            _context.Fill(input, this, botClient, update);

            if (CommandStepsQueue.Count > 0)
            {
                return ProcessCommandStep(); 
            }

            var commandKey = input;
            _availableCommands.TryGetValue(commandKey, out var command);

            if (command == null)
                command = new HelpBotCommand();

            return command.ExecuteAsync(_context);
        }

        public Task ProccessCallbackQueryInput(string input, ITelegramBotClient botClient, Update update)
        {
            _context.Fill(input, this, botClient, update);

            if (CommandStepsQueue.Count > 0)
            {
                return ProcessCommandStep();
            }

            var commandKey = input;
            _availableCommands.TryGetValue(commandKey, out var command);

            if (command == null)
                command = new HelpBotCommand();

            return command.ExecuteAsync(_context);
        }

        private Task ProcessCommandStep()
        {
            return CommandStepsQueue.First().ExecuteAsync(_context);
        }
    }
}