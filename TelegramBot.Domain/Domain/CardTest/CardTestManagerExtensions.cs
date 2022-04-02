using System.Text;
using TelegramBot.Domain.Domain.Test;

public static class CardTestManagerExtensions
{
    public static string[] GetAllTestNames(this CardTestManager manager, params string[] others)
    {
        var testNames = manager.Tests.Select(x => x.Key).ToList();
        testNames.AddRange(others);
        return testNames.ToArray();
    }

    public static bool IsTestCollectionEmpty(this CardTestManager manager)
    {
        return manager.Tests.Count == 0;
    }

    public static bool IsContainsTest(this CardTestManager manager, string testKey)
    {
        return manager.Tests.ContainsKey(testKey);
    }

    public static void ChooseCurrentTest(this CardTestManager manager, string testKey)
    { 
        manager.CurrentTest = manager.Tests[testKey];
    }
}