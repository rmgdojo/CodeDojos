using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;

namespace RMGChess.Core
{
    public static class PGNConverter
    {
        private const string EVENT_IDENTIFIER = "[Event \"";
        private const string DATE_IDENTIFIER = "[Date \"";
        private const string WHITE_IDENTIFIER = "[White \"";
        private const string BLACK_IDENTIFIER = "[Black \"";
        private const string HEADER_END = "\"]";

        public static IList<GameRecord> GetGameRecordsFromPGN(string pgnData)
        {
            List<GameRecord> games = new();
            IList<(string[] HeaderBlock, string MovesBlock)> gameBlocks = GetBlocksFromPGN(pgnData);

            foreach (var gameBlock in gameBlocks)
            {
                // only take the headers we want
                string eventName = gameBlock.HeaderBlock.GetValueFromHeader(EVENT_IDENTIFIER);
                string gameDate = gameBlock.HeaderBlock.GetValueFromHeader(DATE_IDENTIFIER);
                string playingWhite = gameBlock.HeaderBlock.GetValueFromHeader(WHITE_IDENTIFIER);
                string playingBlack = gameBlock.HeaderBlock.GetValueFromHeader(BLACK_IDENTIFIER);

                if (String.IsNullOrWhiteSpace(gameBlock.MovesBlock))
                {
                    throw new InvalidDataException("Game moves block is empty."); // unlikely, but bad data is bad
                }

                string[] moves = gameBlock.MovesBlock.Split(" ").Where(x => !x.Contains(".")).SkipLast(1).ToArray();
                string name = $"{eventName} | {gameDate} | {playingWhite} vs {playingBlack}";

                games.Add(new GameRecord(
                    eventName, 
                    String.IsNullOrWhiteSpace(gameDate) ? DateTime.MinValue : DateTime.Parse(gameDate), 
                    playingWhite, 
                    playingBlack, 
                    moves));
            }

            return games;
        }

        private static string GetValueFromHeader(this string[] pgnLines, string name)
        {
            string line = pgnLines.FirstOrDefault(x => x.Contains(name));
            return line?.Replace(name, "").Replace(HEADER_END, "") ?? "";
        }

        private static IList<(string[] HeaderBlock, string MovesBlock)> GetBlocksFromPGN(string pgnData)
        {
            List<(string[] HeaderBlock, string MovesBlock)> gameBlocks = new();

            pgnData = pgnData.TrimStart().TrimEnd('\r', '\n');
            if (pgnData.StartsWith("["))
            {
                int lineIndex = 0;
                string[] lines = pgnData.Split('\n').Select(x => x.TrimEnd('\r')).ToArray();

                do
                {
                    List<string> headerBlock = new();
                    StringBuilder movesBlock = new();

                    while (lines[lineIndex].StartsWith("["))
                    {
                        headerBlock.Add(lines[lineIndex++].Trim());
                    }

                    if (lines[lineIndex] == "") lineIndex++; // skip the empty line after the header block, if it exists

                    while (lineIndex < lines.Length && lines[lineIndex] != "" && !lines[lineIndex].StartsWith("["))
                    {
                        movesBlock.Append(lines[lineIndex++].Replace("\n", "")); // in case the moves are on multiple lines
                    }

                    if (lineIndex < lines.Length && lines[lineIndex] == "")
                    {
                        while (lines[lineIndex] == "")
                        {
                            lineIndex++; // skip any additional empty lines after the moves block
                        }
                    }

                    // replace all sequences of multiple spaces with a single space
                    movesBlock = new StringBuilder(System.Text.RegularExpressions.Regex.Replace(movesBlock.ToString(), @"\s+", " "));

                    gameBlocks.Add((headerBlock.ToArray(), movesBlock.ToString()));
                    lines = lines.Skip(lineIndex).ToArray(); // remove the processed lines
                    lineIndex = 0; // reset line index for the next game
                }
                while (lineIndex < lines.Length && lines[lineIndex].StartsWith("["));

            }
            else
            {
                throw new InvalidDataException("Invalid PGN data format. Expected to start with a header block.");
            }

            return gameBlocks;
        }
    }
}
