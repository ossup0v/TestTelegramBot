using System.Text;
using TelegramBot.Test;

public static class TestManagerExtensions
{
    public static string[] GetAllTestNames(this TestManager manager)
    {
        return manager.Tests.Select(x => x.Key).ToArray();
    }

    public static bool IsTestCollectionEmpty(this TestManager manager)
    {
        return manager.Tests.Count == 0;
    }

    public static bool IsContainsTest(this TestManager manager, string testKey)
    {
        return manager.Tests.ContainsKey(testKey);
    }

    public static void ChooseCurrentTest(this TestManager manager, string testKey)
    { 
        manager.CurrentTest = manager.Tests[testKey];
    }
}