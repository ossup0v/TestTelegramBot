using TelegramBot.Domain.Domain.Chess.Map;

namespace TelegramBot.Domain.Domain.Chess.Figures
{
    public abstract class ChessFigureBase
    {
        public ChessFigureBase(string mark, long ownerId, Point startPosition, ChessGameSide side)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Mark = mark;
            Position = startPosition;
            Side = side;
        }

        public Guid Id { get; }
        public long OwnerId { get; }
        public string Mark { get; }
        public Point Position { get; protected set; }
        public ChessGameSide Side { get; }

        public void SetPosition(Point newPosition)
        {
            Position = newPosition;
        }
        public abstract string[,] GetAllAvaiblePositionsToMove(ChessMap map);
        public abstract bool TryMove(ChessMap map, Point wontedPosition);
    }
}
