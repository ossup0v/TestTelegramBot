using TelegramBot.Domain.Domain.Chess.Map;

namespace TelegramBot.Domain.Domain.Chess
{
    public static class ChessPositionsHelper
    {
        public static List<Point> GetBlackPawnAvaialablePositions(Point source, ChessMap map, ChessGameSide side, bool isMoved)
        {
            var defaultMap = map.GetDefault();
            int x = source.X, y = source.Y;
            int xLenght = defaultMap.GetLength(0), yLenght = defaultMap.GetLength(1);

            Func<int, int, bool> checkForOut = (x, y) =>
            {
                return x >= 0 && y >= 0 && yLenght > y && xLenght > x;
            };
            var result = new List<Point>(4);


            if (checkForOut(x, y + 1) && defaultMap[y + 1, x] == ChessMapConstants.Empty)
            {
                result.Add(new Point(source.X, source.Y + 1));

                if (isMoved is false &&
                    checkForOut(x, y + 2) && defaultMap[y + 2, x] == ChessMapConstants.Empty)
                {
                    result.Add(new Point(source.X, source.Y + 2));
                }
            }

            var figure = map.GetFigure(new Point(x - 1, y + 1));
            if (checkForOut(x - 1, y + 1) && (figure != null && figure.Side != side))
                result.Add(new Point(source.X - 1, source.Y + 1));

            figure = map.GetFigure(new Point(x + 1, y + 1));
            if (checkForOut(x + 1, y + 1) && (figure != null && figure.Side != side))
                result.Add(new Point(source.X + 1, source.Y + 1));

            return result;
        }

        public static List<Point> GetWhitePawnAvaialablePositions(Point source, ChessMap map, ChessGameSide side, bool isMoved)
        {
            var defaultMap = map.GetDefault();
            int x = source.X, y = source.Y;
            int xLenght = defaultMap.GetLength(0), yLenght = defaultMap.GetLength(1);

            Func<int, int, bool> checkForOut = (x, y) =>
            {
                return x >= 0 && y >= 0 && yLenght > y && xLenght > x;
            };
            var result = new List<Point>(4);

            if (checkForOut(x, y - 1) && defaultMap[y - 1, x] == ChessMapConstants.Empty)
            {
                result.Add(new Point(source.X, source.Y - 1));

                if (isMoved is false &&
                    checkForOut(x, y - 2) && defaultMap[y - 2, x] == ChessMapConstants.Empty)
                {
                    result.Add(new Point(source.X, source.Y - 2));
                }
            }

            var figure = map.GetFigure(new Point(x - 1, y - 1));
            if (checkForOut(x - 1, y - 1) && (figure != null && figure.Side != side))
                result.Add(new Point(source.X - 1, source.Y - 1));

            figure = map.GetFigure(new Point(x + 1, y - 1));
            if (checkForOut(x + 1, y - 1) && (figure != null && figure.Side != side))
                result.Add(new Point(source.X + 1, source.Y - 1));

            return result;
        }

        public static List<Point> GetRookAvaialablePositions(Point source, ChessMap map, ChessGameSide side)
        {
            var defualtMap = map.GetDefault();
            var result = new List<Point>(10);

            for (int x = source.X - 1; x >= 0; x--)
            {
                int y = source.Y;

                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
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
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
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
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
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
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            return result;
        }

        public static List<Point> GetElephantAvaialablePositions(Point source, ChessMap map, ChessGameSide side)
        {
            var defualtMap = map.GetDefault();
            var result = new List<Point>(10);

            for (int y = source.Y + 1, x = source.X + 1; y < defualtMap.GetLength(0) && x < defualtMap.GetLength(1); y++, x++)
            {
                var figure = map.GetFigure(new Point(x, y));
                if (defualtMap[y, x] != ChessMapConstants.Empty)
                {
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
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
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
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
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
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
                    if (figure != null && figure.Side == side)
                    {
                        break;
                    }
                    else if (figure != null && figure.Side != side)
                    {
                        result.Add(new Point(x, y));
                        break;
                    }
                }

                result.Add(new Point(x, y));
            }

            return result;
        }

        public static List<Point> GetQueenAvaialablePositions(Point source, ChessMap map, ChessGameSide side)
        {
            var result = GetElephantAvaialablePositions(source, map, side);
            result.AddRange(GetRookAvaialablePositions(source, map, side));

            return result;
        }

        public static List<Point> GetKingAvailablePositions(Point source, ChessMap map, ChessGameSide side)
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
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            return result;
        }

        public static List<Point> GetKnightAvailablePositions(Point source, ChessMap map, ChessGameSide side)
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
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 2; newX = x - 1;
            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 2; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 1; newX = x + 2;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x + 2;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y + 2; newX = x + 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y + 2; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y + 1; newX = x - 2;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            newY = y - 2; newX = x - 1;

            figure = map.GetFigure(new Point(newX, newY));
            if (checkForOut(newX, newY) && (defaultMap[newY, newX] == ChessMapConstants.Empty || !(figure != null && figure.Side == side)))
                result.Add(new Point(newX, newY));

            return result;
        }
    }
}
