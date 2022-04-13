using TelegramBot.Domain.Domain.Chess.Map;

namespace TelegramBot.Domain.Domain.Chess
{
    public sealed class ChessGame
    {
        public Guid Id { get; }
        public ChessPlayer BlackPlayer { get; private set; }
        public ChessPlayer WhitePlayer { get; private set; }


        private List<ChessPlayer> _players;
        private ChessMap _map;
        private ChessGameSide _currentMoveSide = ChessGameSide.White;

        public ChessGameSide GetCurrentMoveSide() => _currentMoveSide;

        public Action<long> GameEnded = delegate { };
        public Action GameStarted = delegate { };
        public Action MapUpdate = delegate { };

        public ChessGame(ChessPlayer whitePlayer)
        {
            WhitePlayer = whitePlayer;
            Id = Guid.NewGuid();
        }

        public void ConnectPlayer(ChessPlayer blackPlayer)
        {
            _players = new List<ChessPlayer> { WhitePlayer, blackPlayer };
            BlackPlayer = blackPlayer;

            _map = new ChessMap(BlackPlayer.UserId, WhitePlayer.UserId);
            GameStarted();
        }

        public string[,] GetDefultMap()
        {
            return _map?.GetDefault() ?? new string[0, 0];
        }

        public void SurrenderGameBy(long surrenderPlayerId)
        {
            GameEnded(surrenderPlayerId);
        }

        public bool IsHaveTargetFigure(long playerId, ChessGameSide side)
        {
            var targetPlayer = _players.FirstOrDefault(p => p.UserId == playerId && p.Side == side);

            return targetPlayer.ChoosedFigure != null;
        }

        public string[,] TryChooseTargetFigure(long playerId, ChessGameSide side, Point position)
        {
            var targetPlayer = _players.FirstOrDefault(p => p.UserId == playerId && p.Side == side);

            var targetFigure = _map.GetFigure(position, playerId);

            if (targetFigure == null || targetFigure.OwnerId != playerId
             || targetPlayer == null || targetPlayer.ChoosedFigure?.Id == targetFigure.Id)
            {
                targetPlayer?.SetChoosedFigure(null);
                return _map.GetDefault();
            }

            targetPlayer.SetChoosedFigure(targetFigure);

            return targetFigure.GetAllAvaiblePositionsToMove(_map);
        }

        public bool TryToMoveOrChooseFigure(long playerId, ChessGameSide side, Point wontedPosition)
        {
            var player = _players.First(player => player.UserId == playerId && player.Side == side);
            var targetFigure = player.ChoosedFigure;

            if (targetFigure == null)
            {
                return false;
            }

            if (player.Side != _currentMoveSide)
            {
                return false;
            }

            if (!targetFigure.TryMove(_map, wontedPosition))
            {
                player.SetChoosedFigure(null);
                return false;
            }

            MapUpdate();
            SwitchMoveSide();
            player.SetChoosedFigure(null);
            return true;
        }

        private void SwitchMoveSide()
        {
            switch (_currentMoveSide)
            {
                case ChessGameSide.Unexpected:
                    break;
                case ChessGameSide.Black:
                    _currentMoveSide = ChessGameSide.White;
                    break;
                case ChessGameSide.White:
                    _currentMoveSide = ChessGameSide.Black;
                    break;
                default:
                    break;
            }
        }
    }

    public enum ChessGameSide
    {
        Unexpected = 0,
        Black = 1,
        White
    }
}