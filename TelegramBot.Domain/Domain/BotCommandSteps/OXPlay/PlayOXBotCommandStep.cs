using TelegramBot.BotCommands;
using TelegramBot.BotCommandSteps;
using TelegramBot.Domain.Domain.OXPlay;

namespace TelegramBot.Domain.Domain.BotCommandSteps.OXPlay
{
    public sealed class PlayOXBotCommandStep : IBotCommandStep
    {
        private bool _isInited = false;
        private OXGame _game;
        private UserOXPlayer _userPlayer;
        private UserOXPlayer _botPlayer;
        private int _seed = 1;
        private Random _random;
        private CommandExecutionContext _context;

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            _context = context;

            if (Guid.TryParse(context.RawInput, out var gameId))
            {
                TryConnectGame(gameId);
                _isInited = true;
            }

            if (!_isInited)
            {
                Init();
                _isInited = true;
            }


            try
            {
                if (!_game.IsCanMove(_userPlayer.Id))
                {
                    return SendGameMap("Ждём хода игрока");
                }

                var XY = context.RawInput.Split(':');
                var point = new Point(int.Parse(XY[0]), int.Parse(XY[1]));
                if (!_userPlayer.TryChoosePosition(point))
                    return Task.CompletedTask;

                //if (_game.IsGameOver(out var winner))
                //{
                //    context.RemoveCommandStep(this);
                //}

                //while (true)
                //{
                //    var targetBotPoint = new Point(_random.Next(3), _random.Next(3));
                //
                //    if (_botPlayer.TryChoosePosition(targetBotPoint))
                //        break;
                //}
            }
            catch (Exception ex) { }

            return Task.CompletedTask;
        }

        private Task SendGameMap(string title)
        { 
            return _context.SendReply(title + Environment.NewLine + $"`{_game.Id}`", 3, _game.GetMap());
        }

        private void UpdateGameMap(string winner, bool haveAvailablePlaces)
        {
            if (winner == default && !haveAvailablePlaces)
            {
                SendGameMap("💩💩💩💩ОБОСРАЛИСЬ ОБА💩💩💩💩");
                _context.RemoveCommandStep(this);
                return;
            }

            if (winner == default)
            {
                SendGameMap("OX GAME");
                return;
            }


            _context.RemoveCommandStep(this);
            var _ = winner == _userPlayer.PlayerChar ? SendGameMap("🥳🥳🥳ЭТА ПАБЕДА🥳🥳🥳") : SendGameMap("💩💩💩ЭТО ПРОИГРЫШ С ПОДЛИВОЙ💩💩💩");
        }

        private void Init()
        {
            _random = new Random(_seed);
            _userPlayer = new UserOXPlayer(Guid.NewGuid(), "👄", true);//X
            _botPlayer = new UserOXPlayer(Guid.NewGuid(), "💩", false);//O
            _game = new OXGame(new List<OXPlayerBase>()
            {
                _userPlayer,
                _botPlayer
            });
            OXGameStorage.Instance.AddGame(_game);
            _game.MapChanged += UpdateGameMap;
        }

        private bool TryConnectGame(Guid gameId)
        {
            var game = OXGameStorage.Instance.GetGame(gameId);

            if (game == null)
            {
                return false;
            }

            game.MapChanged += UpdateGameMap;
            _userPlayer = new UserOXPlayer(Guid.NewGuid(), "💩", true);
            _game = game;
            _game.ReplaceBotWithUser(_userPlayer);
            SendGameMap("OX GAME");
            return true;
        }
    }
}
