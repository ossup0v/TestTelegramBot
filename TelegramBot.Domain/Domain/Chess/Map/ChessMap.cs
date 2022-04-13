using System.Diagnostics.CodeAnalysis;
using TelegramBot.Domain.Domain.Chess.Figures;

namespace TelegramBot.Domain.Domain.Chess.Map
{
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
            _figures.Add(new RookChessFigure(ChessMapConstants.RookBlackChessMark, playerBlackSide, new Point(0, 0), ChessGameSide.Black));
            _figures.Add(new RookChessFigure(ChessMapConstants.RookBlackChessMark, playerBlackSide, new Point(7, 0), ChessGameSide.Black));

            _map[0, 2] = ChessMapConstants.ElephantBlackChessMark;
            _map[0, 5] = ChessMapConstants.ElephantBlackChessMark;
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantBlackChessMark, playerBlackSide, new Point(2, 0), ChessGameSide.Black));
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantBlackChessMark, playerBlackSide, new Point(5, 0), ChessGameSide.Black));

            _map[0, 1] = ChessMapConstants.KnightBlackChessMark;
            _map[0, 6] = ChessMapConstants.KnightBlackChessMark;
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightBlackChessMark, playerBlackSide, new Point(1, 0), ChessGameSide.Black));
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightBlackChessMark, playerBlackSide, new Point(6, 0), ChessGameSide.Black));

            _map[0, 4] = ChessMapConstants.QueenBlackChessMark;
            _figures.Add(new QueenChessFigure(ChessMapConstants.QueenBlackChessMark, playerBlackSide, new Point(4, 0), ChessGameSide.Black));

            _map[0, 3] = ChessMapConstants.KingBlackChessMark;
            _figures.Add(new KingChessFigure(ChessMapConstants.KingBlackChessMark, playerBlackSide, new Point(3, 0), ChessGameSide.Black));

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
            _figures.Add(new RookChessFigure(ChessMapConstants.RookWhiteChessMark, playerWhiteSide, new Point(0, 7), ChessGameSide.White));
            _figures.Add(new RookChessFigure(ChessMapConstants.RookWhiteChessMark, playerWhiteSide, new Point(7, 7), ChessGameSide.White));


            _map[7, 2] = ChessMapConstants.ElephantWhiteChessMark;
            _map[7, 5] = ChessMapConstants.ElephantWhiteChessMark;
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantWhiteChessMark, playerWhiteSide, new Point(2, 7), ChessGameSide.White));
            _figures.Add(new ElephantChessFigure(ChessMapConstants.ElephantWhiteChessMark, playerWhiteSide, new Point(5, 7), ChessGameSide.White));

            _map[7, 1] = ChessMapConstants.KnightWhiteChessMark;
            _map[7, 6] = ChessMapConstants.KnightWhiteChessMark;
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightWhiteChessMark, playerWhiteSide, new Point(1, 7), ChessGameSide.White));
            _figures.Add(new KnightChessFigure(ChessMapConstants.KnightWhiteChessMark, playerWhiteSide, new Point(6, 7), ChessGameSide.White));

            _map[7, 4] = ChessMapConstants.QueenWhiteChessMark;
            _figures.Add(new QueenChessFigure(ChessMapConstants.QueenWhiteChessMark, playerWhiteSide, new Point(4, 7), ChessGameSide.White));

            _map[7, 3] = ChessMapConstants.KingWhiteChessMark;
            _figures.Add(new KingChessFigure(ChessMapConstants.KingWhiteChessMark, playerWhiteSide, new Point(3, 7), ChessGameSide.White));
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

            var figureExists = GetFigure(to);

            if (figureExists != null) //съели фигуру
            {
                _figures.Remove(figureExists);
            }

            _map[to.Y, to.X] = figure.Mark;
            figure.SetPosition(to);
        }
    }
}
