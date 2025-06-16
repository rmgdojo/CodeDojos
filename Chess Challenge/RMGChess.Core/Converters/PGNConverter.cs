using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core.Converters
{
    public static class PGNConverter
    {
        public static Dictionary<string, string[]> ConvertGame(string input)
        {
            var pgnLines = input.Split("\n");
            var gameStart = Array.FindIndex(pgnLines, x => x == "");
            var gameMovesString = pgnLines[gameStart + 1];
            var gameMoves = gameMovesString.Split(" ").Where(x => !x.Contains(".")).SkipLast(1);


            return new Dictionary<string, string[]> { { "Test Game", gameMoves.ToArray() } };
        }
    }
}
