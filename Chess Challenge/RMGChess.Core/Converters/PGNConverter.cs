namespace RMGChess.Core.Converters
{
    public static class PGNConverter
    {
        private const string EventNameIdentifier = "[Event \"";
        private const string DateIdentifier = "[Date \"";
        private const string UtcTimeIdentifier = "[UTCTime \"";
        private const string HeaderEnd = "\"]";

        public static Dictionary<string, string[]> ConvertGame(string input)
        {
            var pgnLines = input.Split("\n");

            var gameName = pgnLines.FirstOrDefault(x => x.Contains(EventNameIdentifier))?.Replace(EventNameIdentifier, string.Empty).Replace(HeaderEnd, string.Empty);
            var gameDate = pgnLines.FirstOrDefault(x => x.Contains(DateIdentifier))?.Replace(DateIdentifier, string.Empty).Replace(HeaderEnd, string.Empty);
            var gameTime = pgnLines.FirstOrDefault(x => x.Contains(UtcTimeIdentifier))?.Replace(UtcTimeIdentifier, string.Empty).Replace(HeaderEnd, string.Empty);

            var gameStart = Array.FindIndex(pgnLines, x => x == "");
            var gameMovesString = pgnLines[gameStart + 1];
            var gameMoves = gameMovesString.Split(" ").Where(x => !x.Contains(".")).SkipLast(1);

            return new Dictionary<string, string[]> { { $"{gameName} {gameDate} {gameTime}", gameMoves.ToArray() } };
        }

        public static List<Dictionary<string, string[]>> ConvertGames(string result)
        {
            var games = result.Split(new[] { "\n\n\n" }, StringSplitOptions.None);
            return games.Where(x => !string.IsNullOrWhiteSpace(x)).Select(ConvertGame).ToList();
        }
    }
}
