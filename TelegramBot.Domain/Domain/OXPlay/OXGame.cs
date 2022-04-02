namespace TelegramBot.Domain.Domain.OXPlay
{
    public sealed class OXGame
    {
        private List<OXPlayerBase> _players;

        private int _currentPlayerIndex;
        private OXPlayerBase _currentPlayer => _players[_currentPlayerIndex];
        private OXMap _map;

        public Guid Id { get; private set; }
        public event Action<string, bool> MapChanged = delegate { };

        public OXGame(List<OXPlayerBase> players)
        {
            _players = players;

            foreach (var player in players)
                player.SetGame(this);

            _map = new OXMap();
            Id = Guid.NewGuid();
        }

        public bool TryChoosePosition(Point targetPoint, OXPlayerBase player)
        {
            var isCurrentPlayer = player.Id == _currentPlayer.Id;

            if (isCurrentPlayer is false)
                return false;

            if (!_map.TryChooseTargetPosition(targetPoint, player.PlayerChar))
                return false;

            SwitchPlayer();
            IsGameOver(out string winner);
            MapChanged(winner, _map.IsHaveAvailablePlace());
            return true;
        }

        public bool IsCanMove(Guid playerId)
        { 
            return _currentPlayer.Id == playerId;
        }

        public bool IsGameOver(out string winner)
        {
            if (!_map.IsGameOver(out winner))
            {
                winner = default;
                return false;
            }

            return true;
        }

        public string[] GetMap()
        {
            var result = new string[9];
            var index = 0;
            for (int x = 0; x < _map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.Map.GetLength(1); y++)
                {
                    result[index] = _map.Map[x, y].ToString();
                    index++;
                }
            }

            return result;
        }

        private void SwitchPlayer()
        {
            _currentPlayerIndex++;
            if (_currentPlayerIndex == _players.Count)
            {
                _currentPlayerIndex = 0;
            }
        }

        public void ReplaceBotWithUser(UserOXPlayer userPlayer)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (!_players[i].IsRealUser)
                {
                    _players[i] = userPlayer;
                    userPlayer.SetGame(this);
                }
            }
        }
    }

    public class OXMap
    {
        private string[,] _map;

        private const string DefaultChar = "👁";

        public OXMap()
        {
            _map = new string[3, 3];
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    _map[x, y] = DefaultChar;
                }
            }
        }

        public string[,] Map => _map;

        public bool TryChooseTargetPosition(Point targetPosition, string playerChar)
        {
            if (!IsAvailablePosition(targetPosition))
                return false;

            if (IsGameOver(out _))
                return false;

            _map[targetPosition.Y, targetPosition.X] = playerChar;
            return true;
        }

        private bool IsAvailablePosition(Point targetPosition)
        {
            if (targetPosition.X < 0 || targetPosition.Y < 0
                || targetPosition.X > _map.GetLength(0) || targetPosition.Y > _map.GetLength(1))
            {
                return false;
            }

            var isAvailable = _map[targetPosition.Y, targetPosition.X] == DefaultChar;

            return isAvailable;
        }

        public bool IsGameOver(out string winner)
        {
            winner = default;

            if (IsRowWin(0, out winner))
                return true;
            if (IsRowWin(1, out winner))
                return true;
            if (IsRowWin(2, out winner))
                return true;

            if (IsColumnWin(0, out winner))
                return true;
            if (IsColumnWin(1, out winner))
                return true;
            if (IsColumnWin(2, out winner))
                return true;

            if (IsHaveAvailablePlace() is false)
            {
                winner = default;
                return true;
            }

            return false;
        }

        private bool IsRowWin(int column, out string winner)
        {
            winner = _map[column, 0];

            var firstChar = _map[column, 0];

            if (firstChar == DefaultChar)
                return false;

            return _map[column, 1] == firstChar && _map[column, 2] == firstChar;
        }

        private bool IsColumnWin(int row, out string winner)
        {
            winner = _map[0, row];

            var firstChar = _map[0, row];

            if (firstChar == DefaultChar)
                return false;

            return _map[1, row] == firstChar && _map[2, row] == firstChar;
        }

        public bool IsHaveAvailablePlace()
        {
            for (int y = 0; y < _map.GetLength(0); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                    if (_map[y, x] == DefaultChar)
                        return true;

            return false;
        }
    }

    public abstract class OXPlayerBase
    {
        public Guid Id { get; }
        public string PlayerChar { get; }
        public bool IsRealUser { get; }

        protected OXGame _game;

        public OXPlayerBase(Guid id, string playerChar, bool isRealUser)
        {
            Id = id;
            PlayerChar = playerChar;
            IsRealUser = isRealUser;
        }

        public void SetGame(OXGame game)
        {
            _game = game;
        }

        public abstract bool TryChoosePosition(Point position);
    }


    public class UserOXPlayer : OXPlayerBase
    {
        public UserOXPlayer(Guid id, string playerChar, bool isRealUser) : base(id, playerChar, isRealUser) { }

        public override bool TryChoosePosition(Point position)
        {
            return _game.TryChoosePosition(position, this);
        }
    }


    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Y;
        public int X;
    }
}
