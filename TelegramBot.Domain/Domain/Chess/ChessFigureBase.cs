using System.Diagnostics.CodeAnalysis;
using TelegramBot.Domain.Domain.OXPlay;

namespace TelegramBot.Domain.Domain.Chess
{
    public abstract class ChessFigureBase
    {
        public ChessFigureBase(string mark, Guid ownerId, Point startPosition)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Mark = mark;
            Position = startPosition;
        }

        public Guid Id { get; }
        public Guid OwnerId { get; }
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

        private List<ChessPlayer> _players;
        private ChessMap _map;
        private ChessGameSide _currentMoveSide = ChessGameSide.White;

        public ChessGameSide GetCurrentMoveSide() => _currentMoveSide;

        public ChessGame(List<ChessPlayer> players)
        {
            Id = Guid.NewGuid();
            _players = players;
            _map = new ChessMap(players.First(p => p.Side == ChessGameSide.Black).Id, players.First(p => p.Side == ChessGameSide.White).Id);
        }

        public string[,] GetDefultMap()
        {
            return _map.GetDefualt();
        }

        public bool IsHaveTargetFigure(Guid playerId)
        {
            var targetPlayer = _players.FirstOrDefault(p => p.Id == playerId);

            return targetPlayer.ChoosedFigure != null;
        }

        public string[,] TryChooseTargetFigure(Guid playerId, Point position)
        {
            var targetPlayer = _players.FirstOrDefault(p => p.Id == playerId);

            var targetFigure = _map.GetFigure(position, playerId);

            if (targetFigure == null || targetFigure.OwnerId != playerId
             || targetPlayer == null || targetPlayer.ChoosedFigure?.Id == targetFigure.Id)
            {
                targetPlayer?.SetChoosedFigure(null);
                return _map.GetDefualt();
            }

            targetPlayer.SetChoosedFigure(targetFigure);

            return targetFigure.GetAllAvaiblePositionsToMove(_map);
        }

        public bool TryToMoveOrChooseFigure(Guid playerId, Point wontedPosition)
        {
            var player = _players.First(_players => _players.Id == playerId);
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
        public Guid Id { get; }
        public ChessFigureBase ChoosedFigure { get; private set; }
        public ChessGameSide Side { get; set; }

        public ChessPlayer(ChessGameSide side)
        {
            Id = Guid.NewGuid();
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

        public ChessMap(Guid playerBlackSide, Guid playerWhiteSide)
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

            _map[0, 4] = ChessMapConstants.QueenBlackChessMark;
            _figures.Add(new QueenChessFigure(ChessMapConstants.QueenBlackChessMark, playerBlackSide, new Point(4, 0)));

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

            _map[7, 4] = ChessMapConstants.QueenWhiteChessMark;
            _figures.Add(new QueenChessFigure(ChessMapConstants.QueenWhiteChessMark, playerWhiteSide, new Point(4, 7)));
        }

        [return: MaybeNull]
        public ChessFigureBase GetFigure(Point position, Guid onwerId)
        {
            var figureEmoji = _map[position.Y, position.X];

            return GetFigure(figureEmoji, onwerId, position);
        }

        public string[,] GetDefualt()
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
        private ChessFigureBase GetFigure(string emoji, Guid onwerId, Point position)
        {
            return _figures.FirstOrDefault(fig => fig.Mark == emoji && fig.Position.X == position.X && fig.Position.Y == position.Y /*&& fig.OwnerId == onwerId*/);
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

        public PawnChessFigure(ChessGameSide side, string mark, Guid ownerId, Point startPosition) : base(mark, ownerId, startPosition)
        {
            _gameSide = side;
        }

        protected IEnumerable<Point> GetAvailablePositionsToMoveInternal()
        {
            if (_isMoved is false)
            {
                if (_gameSide == ChessGameSide.Black)
                {
                    yield return new Point(Position.X, Position.Y + 1);
                    yield return new Point(Position.X, Position.Y + 2);
                }

                if (_gameSide == ChessGameSide.White)
                {
                    yield return new Point(Position.X, Position.Y - 1);
                    yield return new Point(Position.X, Position.Y - 2);
                }
            }
            else
            {
                if (_gameSide == ChessGameSide.Black)
                {
                    yield return new Point(Position.X, Position.Y + 1);
                }

                if (_gameSide == ChessGameSide.White)
                {
                    yield return new Point(Position.X, Position.Y - 1);
                }
            }
        }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var defaultMap = map.GetDefualt();
            var availablePosToMove = GetAvailablePositionsToMoveInternal();

            foreach (var pos in availablePosToMove)
                if (defaultMap[pos.Y, pos.X] == ChessMapConstants.Empty)
                    defaultMap[pos.Y, pos.X] = ChessMapConstants.ShowPath;

            return defaultMap;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefualt();
            if (GetAvailablePositionsToMoveInternal().Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);

            _isMoved = true;
            return true;
        }
    }

    public sealed class PawnBlackChessFigure : PawnChessFigure
    {
        public PawnBlackChessFigure(Guid ownerId, Point startPosition)
            : base(ChessGameSide.Black, ChessMapConstants.PawnBlackChessMark, ownerId, startPosition) { }
    }

    public sealed class PawnWhiteChessFigure : PawnChessFigure
    {
        public PawnWhiteChessFigure(Guid ownerId, Point startPosition)
            : base(ChessGameSide.White, ChessMapConstants.PawnWhiteChessMark, ownerId, startPosition) { }
    }

    public sealed class RookChessFigure : ChessFigureBase
    {
        public RookChessFigure(string mark, Guid ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefualt();

            foreach (var position in ChessPositionsHelper.GetRookAvaialablePositions(Position, map))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefualt();

            if (ChessPositionsHelper.GetRookAvaialablePositions(Position, map).Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }

    public sealed class ElephantChessFigure : ChessFigureBase
    {
        public ElephantChessFigure(string mark, Guid ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefualt();

            foreach (var position in ChessPositionsHelper.GetElephantAvaialablePositions(Position, map))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefualt();

            if (ChessPositionsHelper.GetElephantAvaialablePositions(Position, map).Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }

    public sealed class QueenChessFigure : ChessFigureBase
    {
        public QueenChessFigure(string mark, Guid ownerId, Point startPosition)
            : base(mark, ownerId, startPosition) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefualt();

            foreach (var position in ChessPositionsHelper.GetQueenAvaialablePositions(Position, map))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            var defaultMap = map.GetDefualt();

            if (ChessPositionsHelper.GetQueenAvaialablePositions(Position, map).Contains(wontedPosition) is false
                || defaultMap[wontedPosition.Y, wontedPosition.X] != ChessMapConstants.Empty)
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
        public static List<Point> GetRookAvaialablePositions(Point source, ChessMap map)
        {
            var defualtMap = map.GetDefualt();
            var result = new List<Point>(10);

            for (int x = source.X - 1; x >= 0; x--)
            {
                int y = source.Y;
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            for (int x = source.X + 1; x < defualtMap.GetLength(0); x++)
            {
                int y = source.Y;
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }


            for (int y = source.Y + 1; y < defualtMap.GetLength(0); y++)
            {
                int x = source.X;
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            for (int y = source.Y - 1; y >= 0; y--)
            {
                int x = source.X;
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            return result;
        }

        public static List<Point> GetElephantAvaialablePositions(Point source, ChessMap map)
        {
            var defualtMap = map.GetDefualt();
            var result = new List<Point>(10);

            for (int y = source.Y + 1, x = source.X + 1; y < defualtMap.GetLength(0) && x < defualtMap.GetLength(1); y++, x++)
            {
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            for (int y = source.Y - 1, x = source.X - 1; y >= 0 && x >= 0; y--, x--)
            {
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            for (int y = source.Y - 1, x = source.X + 1; y >= 0 && x < defualtMap.GetLength(1); y--, x++)
            {
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            for (int y = source.Y + 1, x = source.X - 1; y < defualtMap.GetLength(0) && x >= 0; y++, x--)
            {
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                    break;

                result.Add(new Point(x, y));
            }

            return result;
        }

        public static List<Point> GetQueenAvaialablePositions(Point source, ChessMap map)
        {
            var result = GetElephantAvaialablePositions(source, map);
            result.AddRange(GetRookAvaialablePositions(source, map));

            return result;
        }
    }
}