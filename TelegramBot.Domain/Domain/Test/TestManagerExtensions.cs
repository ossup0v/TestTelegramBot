using System.Text;
using TelegramBot.Domain.Domain.Test;

public static class TestManagerExtensions
{
    public static string[] GetAllTestNames(this TestManager manager, params string[] others)
    {
        var testNames = manager.Tests.Select(x => x.Key).ToList();
        testNames.AddRange(others);
        return testNames.ToArray();
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