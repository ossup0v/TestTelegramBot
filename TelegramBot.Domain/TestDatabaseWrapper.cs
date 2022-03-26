using TelegramBot.InternalAPI.Databases;

namespace TelegramBot.Domain
{
    public static class TestDatabaseWrapper
    {
        private static ITestDatabase _database;

        public static void Init(ITestDatabase database)
        {
            _database = database;
        }

        public static ITestDatabase Database => _database;
    }
}
