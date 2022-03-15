using System.Reflection;
using TelegramBot.BotCommands.Attributes;

namespace TelegramBot.BotCommands
{
    public static class AllCommandsHelper
    {
        private static Dictionary<string, IBotCommand> _botCommands;
        public static IReadOnlyDictionary<string, IBotCommand> BotCommands => _botCommands;
        public static readonly (string, string)[] CommandsWithDescription;

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

                if (_botCommands.ContainsKey(command.Key))
                {
                    Console.WriteLine($"Key {command.Key} is not unic!");
                    continue;
                }

                _botCommands.Add(command.Key, command);
            }

            CommandsWithDescription = new (string, string)[_botCommands.Count];
            var i = 0;
            foreach (var command in _botCommands.Values)
            {
                CommandsWithDescription[i] = new(command.Key, command.Description);
                i++;
            }

            //CommandsWithDescription = _botCommands.Select(command => { return new(command.Key, command.Value.Description); }).ToArray();
        }
    }
}