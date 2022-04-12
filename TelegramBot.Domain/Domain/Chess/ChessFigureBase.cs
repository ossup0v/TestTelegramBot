using System.Diagnostics.CodeAnalysis;
using TelegramBot.Domain.Domain.OXPlay;

namespace TelegramBot.Domain.Domain.Chess
{
    public abstract class ChessFigureBase
    {
        public ChessFigureBase(string mark, long ownerId, Point startPosition)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Mark = mark;
            Position = startPosition;
        }

        public Guid Id { get; }
        public long OwnerId { get; }
        public string Mark { get; }
        public Point Position { get; protected set; }

        public void SetPosition(Point newPosition)
        {
            Position = newPosition;
        }
        public abstract string[,] GetAllAvaiblePositionsToMove(ChessMap map);
        public abstract bool TryMove(ChessMap map, Point wontedPosition);
    }

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

        public bool IsHaveTargetFigure(long playerId)
        {
            var targetPlayer = _players.FirstOrDefault(p => p.UserId == playerId);

            return targetPlayer.ChoosedFigure != null;
        }

        public string[,] TryChooseTargetFigure(long playerId, Point position)
        {
            var targetPlayer = _players.FirstOrDefault(p => p.UserId == playerId);

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

        public bool TryToMoveOrChooseFigure(long playerId, Point wontedPosition)
        {
            var player = _players.First(_players => _players.UserId == playerId);
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

    public class ChessPlayer
    {
        public long UserId { get; }
        public ChessFigureBase ChoosedFigure { get; private set; }
        public ChessGameSide Side { get; set; }

        public ChessPlayer(long id, ChessGameSide side)
        {
            UserId = id;
            Side = side;
        }

        public void SetChoosedFigure(ChessFigureBase choosedFigure)
        {
            ChoosedFigure = choosedFigure;
        }
    }

    public enum ChessGameSide
    {
        Unexpected = 0,
        Black = 1,
        White
    }

    public class ChessMap
    {
        private List<ChessFigureBase> _figures = new();
        private string[,] _map;

        private const int X_MAX = 8;
        private const int Y_MAX = 8;

        public ChessMap(long playerBlackSide, long playerWhiteSide)
        {
            _map = new string[Y_MAX, X_MAX];
            for (int y = 0; y < Y_MAX; y++)
            {
                for (int x = 0; x < X_MAX; x++)
                {
                    _map[y, x] = ChessMapConstants.Empty;
                }
            }

            //BLACK
            _map[1, 0] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 1] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 2] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 3] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 4] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 5] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 6] = ChessMapConstants.PawnBlackChessMark;
            _map[1, 7] = ChessMapConstants.PawnBlackChessMark;
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(0, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(1, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(2, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(3, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(4, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(5, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(6, 1)));
            _figures.Add(new PawnBlackChessFigure(playerBlackSide, new Point(7, 1)));

            _map[0, 0] = ChessMapConstants.RookBlackChessMark;
            _map[0, 7] = ChessMapConstants.RookBlackChessMark;
            _figures.Add(new RookChessFigure(ChessMapConstants.RookBlackChessMark, playerBlackSide, new Point(0, 0)));
            _figures.Add(new RookChessFigure(ChessMapConstants.RookBlackChessMark, playerBlackSide, new Point(7, 0)));

            _map[0, 2] = ChessMapConstants.ElephantBlackChessMark;
            _map[0, 5] = ChessMapConstants.ElephantBlackChessMark;
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantBlackChessMark, playerBlackSide, new Point(2, 0)));
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantBlackChessMark, playerBlackSide, new Point(5, 0)));

            _map[0, 1] = ChessMapConstants.KnightBlackChessMark;
            _map[0, 6] = ChessMapConstants.KnightBlackChessMark;
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightBlackChessMark, playerBlackSide, new Point(1, 0)));
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightBlackChessMark, playerBlackSide, new Point(6, 0)));

            _map[0, 4] = ChessMapConstants.QueenBlackChessMark;
            _figures.Add(new QueenChessFigure(ChessMapConstants.QueenBlackChessMark, playerBlackSide, new Point(4, 0)));

            _map[0, 3] = ChessMapConstants.KingBlackChessMark;
            _figures.Add(new KingChessFigure(ChessMapConstants.KingBlackChessMark, playerBlackSide, new Point(3, 0)));

            //WHITE
            _map[6, 0] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 1] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 2] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 3] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 4] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 5] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 6] = ChessMapConstants.PawnWhiteChessMark;
            _map[6, 7] = ChessMapConstants.PawnWhiteChessMark;
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(0, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(1, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(2, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(3, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(4, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(5, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(6, 6)));
            _figures.Add(new PawnWhiteChessFigure(playerWhiteSide, new Point(7, 6)));

            _map[7, 0] = ChessMapConstants.RookWhiteChessMark;
            _map[7, 7] = ChessMapConstants.RookWhiteChessMark;
            _figures.Add(new RookChessFigure(ChessMapConstants.RookWhiteChessMark, playerWhiteSide, new Point(0, 7)));
            _figures.Add(new RookChessFigure(ChessMapConstants.RookWhiteChessMark, playerWhiteSide, new Point(7, 7)));


            _map[7, 2] = ChessMapConstants.ElephantWhiteChessMark;
            _map[7, 5] = ChessMapConstants.ElephantWhiteChessMark;
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantWhiteChessMark, playerWhiteSide, new Point(2, 7)));
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantWhiteChessMark, playerWhiteSide, new Point(5, 7)));

            _map[7, 1] = ChessMapConstants.KnightWhiteChessMark;
            _map[7, 6] = ChessMapConstants.KnightWhiteChessMark;
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightWhiteChessMark, playerWhiteSide, new Point(1, 7)));
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightWhiteChessMark, playerWhiteSide, new Point(6, 7)));

            _map[7, 4] = ChessMapConstants.QueenWhiteChessMark;
            _figures.Add(new QueenChessFigure(ChessMapConstants.QueenWhiteChessMark, playerWhiteSide, new Point(4, 7)));

            _map[7, 3] = ChessMapConstants.KingWhiteChessMark;
            _figures.Add(new KingChessFigure(ChessMapConstants.KingWhiteChessMark, playerWhiteSide, new Point(3, 7)));
        }

        [return: MaybeNull]
        public ChessFigureBase GetFigure(Point position, long onwerId)
        {
            var figureEmoji = _map[position.Y, position.X];

            return GetFigure(figureEmoji, onwerId, position);
        }

        [return: MaybeNull]
        public ChessFigureBase GetFigure(Point position)
        {
            return _figures.FirstOrDefault(f => f.Position == position);
        }

        public string[,] GetDefault()
        {
            var copy = new string[_map.GetLength(0), _map.GetLength(1)];
            for (int y = 0; y < _map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.GetLength(1); x++)
                {
                    copy[y, x] = _map[y, x];
                }
            }

            return copy;
        }

        [return: MaybeNull]
        private ChessFigureBase GetFigure(string emoji, long onwerId, Point position)
        {
            return _figures.FirstOrDefault(fig => fig.Mark == emoji && fig.Position.X == position.X && fig.Position.Y == position.Y && fig.OwnerId == onwerId);
        }

        public void MoveFigure(Guid figureId, Point from, Point to)
        {
            var figure = _figures.First(fig => fig.Id == figureId);

            _map[from.Y, from.X] = ChessMapConstants.Empty;
            _map[to.Y, to.X] = figure.Mark;
            figure.SetPosition(to);
        }
    }

    public abstract class PawnChessFigure : ChessFigureBase
    {
        private bool _isMoved = false;
        private readonly ChessGameSide _gameSide;

        public PawnChessFigure(ChessGameSide side, string mark, long ownerId, Point startPosition) : base(mark, ownerId, startPosition)
        {
            _gameSide = side;
        }

        protected IEnumerable<Point> GetAvailablePositionsToMoveInternal(ChessMap map)
        {
            if (_gameSide == ChessGameSide.Black)
                return ChessPositionsHelper.GetBlackPawnAvaialablePositions(Position, map, OwnerId, _isMoved);
            else
                return ChessPositionsHelper.GetWhitePawnAvaialablePositions(Position, map, OwnerId, _isMoved);
        }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var defaultMap = map.GetDefault();
            var availablePosToMove = GetAvailablePositionsToMoveInternal(map);

            foreach (var pos in availablePosToMove)
                defaultMap[pos.Y, pos.X] = ChessMapConstants.ShowPath;

            return defaultMap;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefault();
            if (GetAvailablePositionsToMoveInternal(map).Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);

            _isMoved = true;
            return true;
        }
    }

    public sealed class PawnBlackChessFigure : PawnChessFigure
    {
        public PawnBlackChessFigure(long ownerId, Point startPosition)
            : base(ChessGameSide.Black, ChessMapConstants.PawnBlackChessMark, ownerId, startPosition) { }
    }

    public sealed class PawnWhiteChessFigure : PawnChessFigure
    {
        public PawnWhiteChessFigure(long ownerId, Point startPosition)
            : base(ChessGameSide.White, ChessMapConstants.PawnWhiteChessMark, ownerId, startPosition) { }
    }

    public sealed class RookChessFigure : ChessFigureBase
    {
        public RookChessFigure(string mark, long ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefault();

            foreach (var position in ChessPositionsHelper.GetRookAvaialablePositions(Position, map, OwnerId))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefault();

            if (ChessPositionsHelper.GetRookAvaialablePositions(Position, map, OwnerId).Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }

    public sealed class ElephantChessFigure : ChessFigureBase
    {
        public ElephantChessFigure(string mark, long ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefault();

            foreach (var position in ChessPositionsHelper.GetElephantAvaialablePositions(Position, map, OwnerId))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefault();

            if (ChessPositionsHelper.GetElephantAvaialablePositions(Position, map, OwnerId).Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }

    public sealed class QueenChessFigure : ChessFigureBase
    {
        public QueenChessFigure(string mark, long ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefault();

            foreach (var position in ChessPositionsHelper.GetQueenAvaialablePositions(Position, map, OwnerId))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefault();

            if (ChessPositionsHelper.GetQueenAvaialablePositions(Position, map, OwnerId).Contains(wontedPosition) is false)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }

    public sealed class KingChessFigure : ChessFigureBase
    {
        public KingChessFigure(string mark, long ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefault();

            foreach (var position in ChessPositionsHelper.GetKingAvailablePositions(Position, map, OwnerId))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefault();

            if (ChessPositionsHelper.GetKingAvailablePositions(Position, map, OwnerId).Contains(wontedPosition) is false)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }

    public sealed class KnightChessFigure : ChessFigureBase
    {
        public KnightChessFigure(string mark, long ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefault();

            foreach (var position in ChessPositionsHelper.GetKnightAvailablePositions(Position, map, OwnerId))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefault();

            if (ChessPositionsHelper.GetKnightAvailablePositions(Position, map, OwnerId).Contains(wontedPosition) is false)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }
    public class ChessMapConstants
    {
        //black
        public const string PawnBlackChessMark = "♟️";
        public const string KingBlackChessMark = "♚";
        public const string QueenBlackChessMark = "♛";
        public const string RookBlackChessMark = "♜";     //ладья
        public const string KnightBlackChessMark = "♞";     // конь
        public const string ElephantBlackChessMark = "♝";
        //white                                        ;
        public const string PawnWhiteChessMark = "♙";
        public const string KingWhiteChessMark = "♔";
        public const string QueenWhiteChessMark = "♕";
        public const string RookWhiteChessMark = "♖";     //ладья
        public const string KnightWhiteChessMark = "♘";     // конь
        public const string ElephantWhiteChessMark = "♗";
        //help
        public const string ShowPath = "*";
        public const string Empty = " ";

    }

    public static class ChessPositionsHelper
    {
        public static List<Point> GetBlackPawnAvaialablePositions(Point source, ChessMap map, long ownerId, bool isMoved)
        {
            var defaultMap = map.GetDefault();
            int x = source.X, y = source.Y;
            int xLenght = defaultMap.GetLength(0), yLenght = defaultMap.GetLength(1);

            Func<int, int, bool> checkForOut = (x, y) =>
            {
                return x >= 0 && y >= 0 && yLenght > y && xLenght > x;
            };
            var result = new List<Point>(4);


            if (checkForOut(x, y + 1) && defaultMap[x, y + 1] == ChessMapConstants.Empty)
            {
                result.Add(new Point(source.X, source.Y + 1));

                if (isMoved is false &&
                    checkForOut(x, y + 2) && defaultMap[x, y + 2] == ChessMapConstants.Empty)
                {
                    result.Add(new Point(source.X, source.Y + 2));
                }
            }

            var figure = map.GetFigure(new Point(x - 1, y + 1));
            if (checkForOut(x - 1, y + 1) && (figure != null && figure.OwnerId != ownerId))
                result.Add(new Point(source.X - 1, source.Y + 1));

            figure = map.GetFigure(new Point(x + 1, y + 1));
            if (checkForOut(x + 1, y + 1) && (figure != null && figure.OwnerId != ownerId))
                result.Add(new Point(source.X + 1, source.Y + 1));

            return result;
        }

        public static List<Point> GetWhitePawnAvaialablePositions(Point source, ChessMap map, long ownerId, bool isMoved)
        {
            var defaultMap = map.GetDefault();
            int x = source.X, y = source.Y;
            int xLenght = defaultMap.GetLength(0), yLenght = defaultMap.GetLength(1);

            Func<int, int, bool> checkForOut = (x, y) =>
            {
                return x >= 0 && y >= 0 && yLenght > y && xLenght > x;
            };
            var result = new List<Point>(4);

            if (checkForOut(x, y - 1) && defaultMap[x, y - 1] == ChessMapConstants.Empty)
            {
                result.Add(new Point(source.X, source.Y - 1));

                if (isMoved is false &&
                    checkForOut(x, y - 2) && defaultMap[x, y - 2] == ChessMapConstants.Empty)
                {
                    result.Add(new Point(source.X, source.Y - 2));
                }
            }

            var figure = map.GetFigure(new Point(x - 1, y - 1));
            if (checkForOut(x - 1, y - 1) && (figure != null && figure.OwnerId != ownerId))
                result.Add(new Point(source.X - 1, source.Y - 1));

            figure = map.GetFigure(new Point(x + 1, y - 1));
            if (checkForOut(x + 1, y - 1) && (figure != null && figure.OwnerId != ownerId))
                result.Add(new Point(source.X + 1, source.Y - 1));

            return result;
        }

        public static List<Point> GetRookAvaialablePositions(Point source, ChessMap map, long ownerId)
        {
            var defualtMap = map.GetDefault();
            var result = new List<Point>(10);

            for (int x = source.X - 1; x >= 0; x--)
            {
                int y = source.Y;

                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            for (int x = source.X + 1; x < defualtMap.GetLength(0); x++)
            {
                int y = source.Y;
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }


            for (int y = source.Y + 1; y < defualtMap.GetLength(0); y++)
            {
                int x = source.X;
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            for (int y = source.Y - 1; y >= 0; y--)
            {
                int x = source.X;

                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            return result;
        }

        public static List<Point> GetElephantAvaialablePositions(Point source, ChessMap map, long ownerId)
        {
            var defualtMap = map.GetDefault();
            var result = new List<Point>(10);

            for (int y = source.Y + 1, x = source.X + 1; y < defualtMap.GetLength(0) && x < defualtMap.GetLength(1); y++, x++)
            {
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            for (int y = source.Y - 1, x = source.X - 1; y >= 0 && x >= 0; y--, x--)
            {
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            for (int y = source.Y - 1, x = source.X + 1; y >= 0 && x < defualtMap.GetLength(1); y--, x++)
            {
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            for (int y = source.Y + 1, x = source.X - 1; y < defualtMap.GetLength(0) && x >= 0; y++, x--)
            {
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.OwnerId == ownerId)
                    {
                        break;
                    }
                    else if (figure != null && figure.OwnerId != ownerId)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            return result;
        }

        public static List<Point> GetQueenAvaialablePositions(Point source, ChessMap map, long owner)
        {
            var result = GetElephantAvaialablePositions(source, map, owner);
            result.AddRange(GetRookAvaialablePositions(source, map, owner));

            return result;
        }

        public static List<Point> GetKingAvailablePositions(Point source, ChessMap map, long ownerId)
        {
            var defaultMap = map.GetDefault();
            var result = new List<Point>(4);

            int x = source.X, y = source.Y;
            int xLenght = defaultMap.GetLength(0), yLenght = defaultMap.GetLength(1);

            Func<int, int, bool> checkForOut = (x, y) =>
            {
                return x >= 0 && y >= 0 && yLenght > y && xLenght > x;
            };

#warning FIX IT
            int newY = y + 1, newX = x + 1;

            var figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            return result;
        }

        public static List<Point> GetKnightAvailablePositions(Point source, ChessMap map, long ownerId)
        {
            var defaultMap = map.GetDefault();
            var result = new List<Point>();

            int x = source.X, y = source.Y;
            int xLenght = defaultMap.GetLength(0), yLenght = defaultMap.GetLength(1);

            Func<int, int, bool> checkForOut = (x, y) =>
            {
                return x >= 0 && y >= 0 && yLenght > y && xLenght > x;
            };

            int newY = y - 1, newX = x - 2;

            var figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 2; newX = x - 1;
            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 2; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x + 2;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x + 2;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y + 2; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y + 2; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x - 2;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            newY = y - 2; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.OwnerId == ownerId)))
                result.Add(new Point(newX, newY));

            return result;
        }
    }
}