using Microsoft.Extensions.Logging;


public class Localizator
{
    private static Dictionary<string, Dictionary<string, string>> Localizations =
        new Dictionary<string, Dictionary<string, string>>
        {
            ["test"] = new Dictionary<string, string>
            {
                ["en"] = "test",
                ["ru"] = "тест"
            },
            [LocalizationConstants.ShowAllAvailableLanguagesStr] = new Dictionary<string, string>
            {
                ["en"] = "All available langugaes",
                ["ru"] = "Все доступные языки"
            },
            [LocalizationConstants.SetTargetLanguageSuccess] = new Dictionary<string, string>
            {
                ["en"] = "Successfully changed target language",
                ["ru"] = "Язык успешно изменён"
            },
            [LocalizationConstants.AllAvailableCommandsIs] = new Dictionary<string, string>
            {
                ["en"] = "All available commands is",
                ["ru"] = "Все доступные команды"
            },
            [LocalizationConstants.TestListIsEmpty] = new Dictionary<string, string>
            {
                ["en"] = "Your test list is empty",
                ["ru"] = "Ваш список тестов пуст"
            },
            [LocalizationConstants.UseToCreateNewTest] = new Dictionary<string, string>
            {
                ["en"] = "Use to create new test",
                ["ru"] = "Используйте для создание нового теста"
            },
            [LocalizationConstants.ChooseTestThatYouWillCheck] = new Dictionary<string, string>
            {
                ["en"] = "Choose test that you will check",
                ["ru"] = "Выберите тест который будите проходить"
            },
            [LocalizationConstants.LooksLike] = new Dictionary<string, string>
            {
                ["en"] = "Looks like",
                ["ru"] = "Выглядит как"
            },
            [LocalizationConstants.TypeHereTestSharedId] = new Dictionary<string, string>
            {
                ["en"] = "Type test shared id",
                ["ru"] = "Напишите общий id"
            },
            [LocalizationConstants.TypeNameOfNewTest] = new Dictionary<string, string>
            {
                ["en"] = "Type name of new test",
                ["ru"] = "Напишите название нового теста"
            },
            [LocalizationConstants.NameOfTestAlreadyExists] = new Dictionary<string, string>
            {
                ["en"] = "Name of test '{0}' already exists, type something different!",
                ["ru"] = "Тест с названием '{0}' уже существует, придумайте иное название!"
            },
            [LocalizationConstants.TypeTestSteps] = new Dictionary<string, string>
            {
                ["en"] = "Type test steps",
                ["ru"] = "Напишите шаги теста"
            },
            [LocalizationConstants.LooksLikeQuestionAnswer] = new Dictionary<string, string>
            {
                ["en"] = "Looks like question - answer",
                ["ru"] = "Выглядит как вопрос - ответ"
            },
            [LocalizationConstants.TypeExitForFinishAddingTestSteps] = new Dictionary<string, string>
            {
                ["en"] = "Type /exit for leave adding test steps",
                ["ru"] = "Напишите /exit для завершение добавления новых шагов теста"
            },
            [LocalizationConstants.ChooseTestThatYouWillDelete] = new Dictionary<string, string>
            {
                ["en"] = "Choose test that you will delete",
                ["ru"] = "Выберите тест который будите удалять"
            },
            [LocalizationConstants.ToCancelType] = new Dictionary<string, string>
            {
                ["en"] = "You can cancal it",
                ["ru"] = "Вы можете прервать удаление"
            },
            [LocalizationConstants.DeletionTestIsCancaled] = new Dictionary<string, string>
            {
                ["en"] = "Test deletion is cancaled",
                ["ru"] = "Удаление теста приостановлено"
            },
            [LocalizationConstants.ChooseTestThatYouWillShare] = new Dictionary<string, string>
            {
                ["en"] = "Choose test that you will share",
                ["ru"] = "Выберите тест которым собираетесь поделиться"
            },
            [LocalizationConstants.ChooseTestThatYouWillEdit] = new Dictionary<string, string>
            {
                ["en"] = "Choose test that you will edit",
                ["ru"] = "Выберите тест который собираетесь изменить"
            },
            [LocalizationConstants.TestWithNameWasCreated] = new Dictionary<string, string>
            {
                ["en"] = "Test with name {0} added. Id to share",
                ["ru"] = "Тест с именем {0} был создан. Id для обмена"
            },
            [LocalizationConstants.TestStepIsNotCorrect] = new Dictionary<string, string>
            {
                ["en"] = "'{0}', is not correct sample is",
                ["ru"] = "'{0}', некорректно, пример:"
            },
            [LocalizationConstants.TryAgain] = new Dictionary<string, string>
            {
                ["en"] = "Try again",
                ["ru"] = "попробуйте снова"
            },
            [LocalizationConstants.TestSuccessfullyDeleted] = new Dictionary<string, string>
            {
                ["en"] = "Test successfully deleted",
                ["ru"] = "Тест успешно удалён"
            },
            [LocalizationConstants.CantFindTestWithName] = new Dictionary<string, string>
            {
                ["en"] = "Can't find test with name {0}",
                ["ru"] = "Не удалось найти тест с именем {0}"
            },
            [LocalizationConstants.ChooseOneOfThisTest] = new Dictionary<string, string>
            {
                ["en"] = "Choose one of this test",
                ["ru"] = "Выберите один из тестов"
            },
            [LocalizationConstants.TestIsDone] = new Dictionary<string, string>
            {
                ["en"] = "Test is done",
                ["ru"] = "Тест завершен"
            },
            [LocalizationConstants.TestIdIs] = new Dictionary<string, string>
            {
                ["en"] = "Test id is",
                ["ru"] = "Id теста"
            },
            [LocalizationConstants.FoundTestWithNameAndId] = new Dictionary<string, string>
            {
                ["en"] = "Found test with name {0} and id",
                ["ru"] = "Найдет тест с именем {0} и id"
            },
            [LocalizationConstants.CantFindTestWithSharedId] = new Dictionary<string, string>
            {
                ["en"] = "Can't find shared test with id {0}",
                ["ru"] = "Ну получилось найти тест с id {0}"
            },
            [LocalizationConstants.CantUnderstandYouPeekOneOfThisCommands] = new Dictionary<string, string>
            {
                ["en"] = "Can't understand you. Peek one of this commands, please",
                ["ru"] = "Не могу понять вас. Выберите одну из команд, пожалуста"
            },
            [LocalizationConstants.ShowCorrectAnswer] = new Dictionary<string, string>
            {
                ["en"] = "Answer",
                ["ru"] = "Ответ"
            },
            [LocalizationConstants.IKnow] = new Dictionary<string, string>
            {
                ["en"] = "I know!",
                ["ru"] = "Я знаю!"
            },
            [LocalizationConstants.IDontKnow] = new Dictionary<string, string>
            {
                ["en"] = "Don't know",
                ["ru"] = "Не знаю"
            }
        };

    private readonly ILogger _logger;
    private readonly Func<string> _getLanguage;

    public Localizator(ILogger logger, Func<string> getLanguage)
    {
        _logger = logger;
        _getLanguage = getLanguage;
    }

    public string GetLocalizedString(string key)
    {
        if (Localizations.ContainsKey(key) is false)
        {
            _logger.LogError($"Can't find key {key} in localizations");
            return string.Empty;
        }

        if (Localizations[key].ContainsKey(_getLanguage()) is false)
        {
            _logger.LogError($"Can't find localization with key {key} for language {_getLanguage()} in localizations");
            return string.Empty;
        }

        return Localizations[key][_getLanguage()];
    }
}