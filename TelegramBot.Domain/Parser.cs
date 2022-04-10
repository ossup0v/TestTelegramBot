namespace TelegramBot.Domain
{
    public static class Parser
    {
        public static Point ToPoint(string source)
        {
            return ToPoint(source, out _);
        }

        public static Point ToPoint(string source, out bool result)
        {
            result = true;
            try
            {
                var XY = source.Split(':');

                result &= int.TryParse(XY[0], out var x);
                result &= int.TryParse(XY[1], out var y);

                return new Point(x, y);
            }
            catch
            { 
                result = false;
                return new Point(-1, -1);
            }
        }
    }
}
