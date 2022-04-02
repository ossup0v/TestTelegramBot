using System.Reflection;
using TelegramBot.Domain.Domain.BotCommands.Common;

namespace TelegramBot.BotCommands
{
    public static class AllCommandsHelper
    {
        private static Dictionary<string, IBotCommand> _botCommands;
        public static IReadOnlyDictionary<string, IBotCommand> BotCommands => _botCommands;

        static AllCommandsHelper()
        {
            var type = typeof(IBotCommand);
            var ignoreAttribute = typeof(NotAvailableCommandAttribute);
            var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsDefined(ignoreAttribute) && type.IsAssignableFrom(p) && p.IsClass).ToList();

            _botCommands = new Dictionary<string, IBotCommand>(commandTypes.Count);

            foreach (var commandType in commandTypes)
            {
                var command = (IBotCommand)Activator.CreateInstance(commandType);

                foreach (var commandkey in command.Keys.Values)
                {
                    if (_botCommands.ContainsKey(commandkey))
                    {
                        Console.WriteLine($"Key {commandkey} is not unic!");
                        continue;
                    }

                    _botCommands.Add(commandkey, command);
                }
            }
        }

        public static string[] GetCommandKeysToShow(CommandExecutionContext context)
        {
            var result = new List<string>();
            foreach (var command in _botCommands.Values)
            {
                var commandKey = command.Keys[context.Client.GetLanguage()];

                if (command.IsCanExecute(context) && !result.Contains(commandKey))
                    result.Add(commandKey);
            }

            return result.ToArray();
        }
    }
}