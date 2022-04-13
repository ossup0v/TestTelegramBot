using TelegramBot.Domain.Domain.Chess.Map;

namespace TelegramBot.Domain.Domain.Chess.Figures
{
    public sealed class ElephantChessFigure : ChessFigureBase
    {
        public ElephantChessFigure(string mark, long ownerId, Point startPosition, ChessGameSide side)
            : base(mark, ownerId, startPosition, side) { }

        public override string[,] GetAllAvaiblePositionsToMove(ChessMap map)
        {
            var result = map.GetDefault();

            foreach (var position in ChessPositionsHelper.GetElephantAvaialablePositions(Position, map, Side))
                result[position.Y, position.X] = ChessMapConstants.ShowPath;

            return result;
        }

        public override bool TryMove(ChessMap map, Point wontedPosition)
        {
            if (ChessPositionsHelper.GetElephantAvaialablePositions(Position, map, Side).Contains(wontedPosition) is false)
                return false;

            map.MoveFigure(Id, Position, wontedPosition);
            return true;
        }
    }
}
