using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BotCommands;
using TelegramBot.BotCommandSteps;
using TelegramBot.Domain.Domain.BotCommandSteps.ChessGame;
using TelegramBot.Domain.Domain.BotCommandSteps.OXPlay;
using TelegramBot.Domain.Domain.OXPlay;

namespace TelegramBot.Domain.Domain.BotCommandSteps
{
    public sealed class ConnectToGameBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            if (Guid.TryParse(context.RawInput, out var gameId))
            {
                if (ChessGameStorage.Contains(gameId))
                {
                    context.RemoveCommandStep(this);
                    context.AddCommandStep(new ChessGamePlayBotCommandStep(context, gameId));
                }

                if (OXGameStorage.Instance.GetGame(gameId) != null)
                {
                    context.RemoveCommandStep(this);
                    context.AddCommandStep(new PlayOXBotCommandStep(context, gameId));
                }
            }

            return Task.CompletedTask;
        }
    }
}
