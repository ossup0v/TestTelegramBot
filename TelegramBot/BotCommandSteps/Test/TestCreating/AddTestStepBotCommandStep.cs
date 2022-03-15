using TelegramBot.BotCommands;

namespace TelegramBot.BotCommandSteps.Test.TestCreating
{
    public class AddTestStepBotCommandStep : IBotCommandStep
    {
        public async Task ExecuteAsync(CommandExecutionContext context)
        {
            if (context.RawInput.ToLower().Contains("exit") && !context.RawInput.Contains('-'))
            {
                context.RemoveCommandStep(this);
                var test = context.Client.TestManager.EndCreateNewTest();
                await context.SendMessage($"Test with name {test.Name} added with id to share {test.Id}");
                return;
            }

            var questionAndAnswer = context.RawInput.Split('-');
            if (questionAndAnswer.Length is not 2)
            {
                await context.SendMessage($"{context.RawInput} is not correct sample is"
                    , "question - answer"
                    , "Type again");
                return;
            }

            var question = questionAndAnswer[0].Trim();
            var answer = questionAndAnswer[1].Trim();

            context.Client.TestManager.AddNewTestStep(question, answer);
        }
    }
}