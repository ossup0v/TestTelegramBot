using TelegramBot.Domain.Domain.Chess.Figures;

namespace TelegramBot.Domain.Domain.Chess
{
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
}
