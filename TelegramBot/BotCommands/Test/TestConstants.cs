namespace TelegramBot.BotCommands.Test
{
    public static class TestConstants
    {
        public const string ShowAnswer = "showanswer";
        public const string CorrectAnswer = "correctasnwer";
        public const string IncorrecntAnswer = "incorrectasnwer";
        public const string ChooseAnswer = $"{ShowAnswer}\n{CorrectAnswer}\n{IncorrecntAnswer}";
        public static string[] ChooseAnswerArray = new string[] { ShowAnswer, CorrectAnswer, IncorrecntAnswer };
        public const string CancelTestDelete = "cancel_";
    }
}
