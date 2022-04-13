using TelegramBot.Domain.Domain.Chess.Map;

namespace TelegramBot.Domain.Domain.Chess.Figures
{
    public abstract class PawnChessFigure : ChessFigureBase
    {
        private bool _isMoved = false;

        public PawnChessFigure(ChessGameSide side, string mark, long ownerId, Point startPosition)
            : base(mark, ownerId, startPosition, side) { }

        protected IEnumerable<Point> GetAvailablePositionsToMoveInternal(ChessMap map)
        {
            if (Side == ChessGameSide.Black)
                return ChessPositionsHelper.GetBlackPawnAvaialablePositions(Position, map, Side, _isMoved);
            else
                return ChessPositionsHelper.GetWhitePawnAvaialablePositions(Position, map, Side, _isMoved);
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
            if (GetAvailablePositionsToMoveInternal(map).Contains(wontedPosition) is false)
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
}
