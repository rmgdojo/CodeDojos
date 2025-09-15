using System.Text;

namespace RMGChess.Core
{
    public static class PGNConverter
    {
        /*
         Example PGN:

            [Event "Rated Bullet game"]
            [Date "2017.12.15"]
            [White "DannyTheDonkey"]
            [Black "chessbrahs"]

            1. d4 Nf6 2. c4 e6 3. Nc3 Bb4 4. e3 O-O 5. Bd3 d5 6. cxd5 exd5 7. Nge2 c6 8. O-O Bd6 9. f3 a5 10. e4 dxe4 11. fxe4 Be7 12. e5 Nd5 
            13. Nxd5 cxd5 14. Be3 Nc6 15. Nf4 g6 16. Qf3 Be6 17. Nxe6 fxe6 18. Qg4 Rf5 19. Rxf5 exf5 20. Bxf5 Kg7 21. Rf1 Qb6 22. h4 Nxe5 
            23. Qg3 Nc4 24. Bf2 Qd6 25. Bd3 Nxb2 26. Be2 Qxg3 27. Bxg3 Bf6 28. Be5 Bxe5 29. dxe5 Nc4 30. e6 Rf8 31. Rb1 Kf6 32. Rxb7 Kxe6 
            33. Rxh7 Ne5 34. Ra7 Kf5 35. Rxa5 Ke4 36. Ra3 d4 37. Ra4 Ke3 38. Bf1 d3 39. Ra5 Rf5 40. Rd5 d2 41. Be2 Kxe2 42. Rxd2+ Kxd2 
            43. Kh2 Ke2 44. Kg3 Nd3 45. Kh3 1-0
        */

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

                // remove move pair numbers and the final score
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
