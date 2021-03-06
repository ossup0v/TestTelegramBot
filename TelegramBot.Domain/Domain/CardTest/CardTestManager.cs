using TelegramBot.InternalAPI.Databases.DTO;
using TelegramBot.InternalAPI.Domain;

namespace TelegramBot.Domain.Domain.Test
{
    public sealed class CardTestManager
    {
        public TestCollection CurrentTest = default;
        public Dictionary<string, TestCollection> Tests = new();

        private TestCollection _newTest = null;
        private readonly long _userIdOwner;

        public CardTestManager(long userIdOwner, List<TestCollection> tests)
        {
            _userIdOwner = userIdOwner;
            Tests = tests.ToDictionary(x => x.Name, x => x);
        }

        public bool TryChooseTest(string testName)
        {
            Tests.TryGetValue(testName, out var test);

            if (test == null)
                return false;

            CurrentTest = test;
            return true;
        }

        public bool StartCreateNewTest(string testName)
        {
            if (Tests.ContainsKey(testName))
                return false;

            _newTest = new TestCollection(testName, Guid.NewGuid(), new List<TestStep>());
            return true;
        }

        public void AddNewTestStep(string question, string answer)
        {
            _newTest.AddStep(new TestStep { Answer = answer, Question = question });
        }

        public TestCollection EndCreateNewTest()
        {
            if (_newTest.GetTestSteps().Count == 0)
                return _newTest;

            Tests.Add(_newTest.Name, _newTest);
            TestDatabaseWrapper.Database.AddTest(new TestCollectionData
            {
                ChatIdOwner = _userIdOwner,
                Id = _newTest.Id,
                Name = _newTest.Name,
                Steps = _newTest.GetTestSteps().Select(x => new TestStepData() { Answer = x.Answer, Question = x.Question }).ToArray(),
            });
            return _newTest;
        }

        public void RemoveTest(string testToDelete)
        {
            Tests.Remove(testToDelete);
            TestDatabaseWrapper.Database.DeleteTest(testToDelete);
        }
    }
}
