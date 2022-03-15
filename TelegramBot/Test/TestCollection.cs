namespace TelegramBot.Test
{
    public sealed class TestCollection
    {
        public Guid Id { get; }
        public string Name { get; init; }

        private readonly List<TestStep> _steps;
        private int _currentStep;
        private int _correctAnswerCount;

        public TestCollection(string name, Guid id, List<TestStep> steps)
        {
            Name = name;
            Id = id;
            _steps = steps;
            _currentStep = 0;
        }

        public void AddStep(TestStep step)
        { 
            _steps.Add(step);
        }

        public bool IsDone => MoveToNext().HasValue is false;

        public void ApplyAnswer(bool isRight = false)
        {
            if (isRight)
                _correctAnswerCount++;
        }

        public TestStep? MoveToNext()
        {
            if (_currentStep < _steps.Count - 1)
            {
                _currentStep++;
                return _steps[_currentStep];
            }
            else
            {
                return null;
            }
        }

        public string GetAnswer() => _steps[_currentStep].Answer ?? String.Empty;
        public string GetQuestion() => _steps[_currentStep].Question ?? String.Empty;
        public int GetCorrectAnswerCount() => _correctAnswerCount;
        public int GetAllQuestionCount() => _steps.Count;

        public void Reset()
        {
            _currentStep = 0;
            _correctAnswerCount = 0;
        }

        public IReadOnlyList<TestStep> GetTestSteps()
        { 
            return _steps;
        }
    }

    public struct TestStep
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
}