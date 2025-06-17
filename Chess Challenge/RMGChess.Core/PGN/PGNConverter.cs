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

        public static IList<GameRecord> ConvertGames(string pgnData)
        {
            List<GameRecord> games = new();

            // we can't automatically assume that the double new line convention etc will be honoured
            // so we need to proactive parse the PGN data into blocks - 1 header, 1 moves for each game
            List<(string[] HeaderBlock, string MovesBlock)> gameBlocks = new();

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

                    while (lines[lineIndex] != "" && !lines[lineIndex].StartsWith("["))
                    {
                        movesBlock.Append(lines[lineIndex++].Replace("\n", "")); // in case the moves are on multiple lines
                    }

                    while (lineIndex < lines.Length && lines[lineIndex] == "")
                    {
                        lineIndex++; // skip any additional empty lines after the moves block
                    }
                    
                    gameBlocks.Add((headerBlock.ToArray(), movesBlock.ToString().TrimEnd()));
                    lines = lines.Skip(lineIndex).ToArray(); // remove the processed lines
                    lineIndex = 0; // reset line index for the next game
                }
                while (lineIndex < lines.Length && lines[lineIndex].StartsWith("["));

            }
            else
            {
                throw new InvalidDataException("Invalid PGN data format. Expected to start with a header block.");
            }

            // now turn blocks into GameRecords
            foreach (var gameBlock in gameBlocks)
            {
                // only take the headers we want
                string gameName = gameBlock.HeaderBlock.GetValueFromHeader(EVENT_IDENTIFIER);
                string gameDate = gameBlock.HeaderBlock.GetValueFromHeader(DATE_IDENTIFIER);
                string playingWhite = gameBlock.HeaderBlock.GetValueFromHeader(WHITE_IDENTIFIER);
                string playingBlack = gameBlock.HeaderBlock.GetValueFromHeader(BLACK_IDENTIFIER);

                if (String.IsNullOrWhiteSpace(gameBlock.MovesBlock)) throw new InvalidDataException("Game moves block is empty."); // unlikely, but bad data is bad data

                string[] moves = gameBlock.MovesBlock.Split(" ").Where(x => !x.Contains(".")).SkipLast(1).ToArray();
                string name = $"{gameName} | {gameDate} | {playingWhite} vs {playingBlack}";

                games.Add(new GameRecord(name, moves));
            }

            return games;
        }

        private static string GetValueFromHeader(this string[] pgnLines, string name)
        {
            string line = pgnLines.FirstOrDefault(x => x.Contains(name));
            return line?.Replace(name, string.Empty).Replace(HEADER_END, string.Empty) ?? string.Empty;
        }
    }
}
