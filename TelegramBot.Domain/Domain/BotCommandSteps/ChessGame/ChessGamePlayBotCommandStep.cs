using TelegramBot.BotCommands;
using TelegramBot.BotCommandSteps;
using TelegramBot.Domain.Domain.Chess;

namespace TelegramBot.Domain.Domain.BotCommandSteps.ChessGame
{
    internal class ChessGamePlayBotCommandStep : IBotCommandStep
    {
        private CommandExecutionContext _context;
        private bool _isInited = false;
        private bool _soloGame = false;
        private Chess.ChessGame _chessGame;
        private ChessPlayer _chessPlayerWhite;
        private ChessPlayer _chessPlayerBlack;
        private int _moveCounter = 0;
        private ChessGameSide _playerSide;

        private ChessPlayer _currentPlayer() 
        {
            if (_soloGame)
            { 
                return _moveCounter % 2 == 0 ? _chessPlayerWhite : _chessPlayerBlack;
            }

            return _playerSide == ChessGameSide.White ? _chessPlayerWhite : _chessPlayerBlack; 
        }

        public ChessGamePlayBotCommandStep(bool soloGame) 
        {
            _soloGame = soloGame;
            _playerSide = ChessGameSide.White;
        }

        public ChessGamePlayBotCommandStep(CommandExecutionContext context, Guid gameId)
        {
            _playerSide = ChessGameSide.Black;
            _context = context;

            TryConnectGame(gameId);
            _isInited = true;
            _soloGame = false;
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

                if (context.RawInput.Contains("draw"))
                {
                    return SendGameMap("Ничья!");
                }

                if (context.RawInput.Contains("surrender"))
                {
                    return SurrenderGame(context.RawInput);
                }

                var point = Parser.ToPoint(context.RawInput, out var result);

                if (result is false)
                { 
                    return SendGameMap();
                }

                if (_chessGame.IsHaveTargetFigure(_currentPlayer().UserId, _currentPlayer().Side) is false)
                {
                    return SendGameMap(_chessGame.TryChooseTargetFigure(_currentPlayer().UserId, _currentPlayer().Side, point));
                }

                if (_chessGame.TryToMoveOrChooseFigure(_currentPlayer().UserId, _currentPlayer().Side, point))
                {
                    SwitchGameSide();
                    return SendGameMap();
                }

                return SendGameMap();
            }
            finally { }
        }

        private void SwitchGameSide()
        {
            _moveCounter++;
        }

        private string[] ToUserMap(string[,] chessMap)
        {
            var result = new List<string>(chessMap.GetLength(0) * chessMap.GetLength(1));

            for (int y = 0; y < chessMap.GetLength(0); y++)
                for (int x = 0; x < chessMap.GetLength(1); x++)
                    result.Add(chessMap[y, x]);

            return result.ToArray();
        }

        private Task SurrenderGame(string rawData)
        {
            if (!rawData.Contains(':'))
                return Task.CompletedTask;

            var userIdStr = rawData.Split(':')[0];
            var userId = long.Parse(userIdStr);

            var surrenderUser = userId == _chessPlayerBlack.UserId ? _chessPlayerBlack : _chessPlayerWhite;

            _chessGame.SurrenderGameBy(surrenderUser.UserId);

            return Task.CompletedTask;
        }

        public void SendGameEnd(long surrenderPlayerId)
        {
            _context.RemoveCommandStep(this);
            var _ = surrenderPlayerId != _context.Client.UserId ? 
                SendGameMap("🥳🥳🥳ЭТА ПАБЕДА🥳🥳🥳") : 
                SendGameMap("💩💩💩ЭТО ПРОИГРЫШ С ПОДЛИВОЙ💩💩💩");
        }

        private Task SendGameMap(string title)
        {
            if (_currentPlayer().Side == ChessGameSide.White || _soloGame)
            {
                return _context.SendReply(title + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(_chessGame.GetDefultMap()));
            }
            else
            { 
                return _context.SendMapInversed(title + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(_chessGame.GetDefultMap()));
            }
        }

        private Task SendGameMap()
        {
            if (_currentPlayer().Side == ChessGameSide.White || _soloGame)
            {
                return SendGameInternal();
            }
            else
            { 
                return SendGameInternalInversed();
            }
        }

        private Task SendGameInternalInversed()
        {
            var currentSideMove = _chessGame.GetCurrentMoveSide() == ChessGameSide.White ? "Ход белых" : "Ход черных";
            return _context.SendMapInversed(currentSideMove + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(_chessGame.GetDefultMap()));
        }
        private Task SendGameInternal()
        {
            var currentSideMove = _chessGame.GetCurrentMoveSide() == ChessGameSide.White ? "Ход белых" : "Ход черных";
            return _context.SendReply(currentSideMove + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(_chessGame.GetDefultMap()));
        }

        private Task SendGameMap(string[,] map)
        {
            var currentSideMove = _chessGame.GetCurrentMoveSide() == ChessGameSide.White ? "Ход белых" : "Ход черных";
            if (_currentPlayer().Side == ChessGameSide.White || _soloGame)
            {
                return _context.SendReply(currentSideMove + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(map));
            }
            else
            { 
                return _context.SendMapInversed(currentSideMove + Environment.NewLine + $"`{_chessGame.Id}`", 8, ToUserMap(map));
            }
        }
        
        private void TryConnectGame(Guid gameId)
        {
            _chessGame = ChessGameStorage.Get(gameId);
            _chessPlayerBlack = new ChessPlayer(_context.Client.UserId, ChessGameSide.Black);

            _chessGame.ConnectPlayer(_chessPlayerBlack);

            _chessPlayerWhite = _chessGame.WhitePlayer;

            if (!_soloGame)
            {
                _chessGame.MapUpdate += OnMapUpdate;
                _chessGame.GameEnded += SendGameEnd;
                SendGameMap();
            }
        }

        private void OnMapUpdate()
        {
            SendGameMap();
        }

        private void SendMapOnStart()
        {
            _chessPlayerBlack = _chessGame.BlackPlayer;
            SendGameMap();
        }

        private void Init()
        {
            _chessPlayerWhite = new ChessPlayer(_context.Client.UserId, ChessGameSide.White);

            _chessGame = new Chess.ChessGame(_chessPlayerWhite);
            _chessGame.GameEnded += SendGameEnd;
            _chessGame.GameStarted += SendMapOnStart;
            _chessGame.MapUpdate += OnMapUpdate;
            ChessGameStorage.Add(_chessGame);

            if (_soloGame)
            {
                TryConnectGame(_chessGame.Id);
            }
        }
    }

    public static class ChessGameStorage
    {
        private static Dictionary<Guid, Chess.ChessGame> _chessGames = new Dictionary<Guid, Chess.ChessGame>();

        public static void Add(Chess.ChessGame game)
        {
            if (_chessGames.ContainsKey(game.Id))
                return;

            _chessGames.Add(game.Id, game);
        }

        public static Chess.ChessGame Get(Guid gameId)
        {
            _chessGames.TryGetValue(gameId, out var game);
         
            return game;
        }

        public static bool Contains(Guid gameId)
        {
            return _chessGames.ContainsKey(gameId);
        }
    }
}
