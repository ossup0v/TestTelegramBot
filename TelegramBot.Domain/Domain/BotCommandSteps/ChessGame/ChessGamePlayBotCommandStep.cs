using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BotCommands;
using TelegramBot.BotCommandSteps;
using TelegramBot.Domain.Domain.Chess;

namespace TelegramBot.Domain.Domain.BotCommandSteps.ChessGame
{
    internal class ChessGamePlayBotCommandStep : IBotCommandStep
    {
        private CommandExecutionContext _context;
        private bool _isInited = false;
        private Chess.ChessGame _chessGame;
        private ChessPlayer _chessPlayerWhite;
        private ChessPlayer _chessPlayerBlack;
        private ChessPlayer _currentPlayer;
        private int _moveCounter = 1;

        private void SwitchPlayer()
        {
            if (_moveCounter % 2 == 0)
                _currentPlayer = _chessPlayerWhite;
            else
                _currentPlayer = _chessPlayerBlack;

            _moveCounter++;
        }

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            _context = context;

            try
            {
                if (_isInited is false)
                {
                    Init();
                    _isInited = true;
                }

                var point = Parser.ToPoint(context.RawInput, out var result);

                if (result is false)
                { 
                    return SendGameMap();
                }

                if (_chessGame.IsHaveTargetFigure(_currentPlayer.Id) is false)
                {
                    return SendGameMap(_chessGame.TryChooseTargetFigure(_currentPlayer.Id, point));
                }

                if (_chessGame.TryToMoveOrChooseFigure(_chessPlayerWhite.Id, point))
                {
                    SwitchPlayer();
                    return SendGameMap();
                }
                else if(_chessGame.TryToMoveOrChooseFigure(_chessPlayerBlack.Id, point))
                {
                    SwitchPlayer();
                    return SendGameMap();
                }

                return SendGameMap();
            }
            finally { }
        }


        private string[] ToUserMap(string[,] chessMap)
        {
            var result = new List<string>(chessMap.GetLength(0) * chessMap.GetLength(1));

            for (int y = 0; y < chessMap.GetLength(0); y++)
            {
                for (int x = 0; x < chessMap.GetLength(1); x++)
                {
                    result.Add(chessMap[y, x]);
                }
            }

            return result.ToArray();
        }

        private Task SendGameMap(string title)
        {
            return _context.SendReply(title + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(_chessGame.GetDefultMap()));
        }

        private Task SendGameMap()
        {
            var currentSideMove = _chessGame.GetCurrentMoveSide() == ChessGameSide.White ? "Ход белых" : "Ход черных";
            return _context.SendReply(currentSideMove + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(_chessGame.GetDefultMap()));
        }

        private Task SendGameMap(string[,] map)
        {
            var currentSideMove = _chessGame.GetCurrentMoveSide() == ChessGameSide.White ? "Ход белых" : "Ход черных";
            return _context.SendReply(currentSideMove + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(map));
        }

        private void Init()
        {
            _chessPlayerBlack = new ChessPlayer(ChessGameSide.Black);
            _chessPlayerWhite = new ChessPlayer(ChessGameSide.White);
            _currentPlayer = _chessPlayerWhite;

            _chessGame = new Chess.ChessGame(new List<ChessPlayer>() { _chessPlayerBlack, _chessPlayerWhite });
        }
    }
}
