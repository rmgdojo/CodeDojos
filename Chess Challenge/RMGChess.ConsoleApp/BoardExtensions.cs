using RMGChess.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.ConsoleApp
{
    public static class BoardExtensions
    {
        public static string GetBoardString(this Board board)
        {
            StringBuilder sb = new StringBuilder();
            for (int rank = 8; rank >= 1; rank--)
            {
                sb.Append(rank);
                sb.Append(" ");
                for (char file = 'a'; file <= 'h'; file++)
                {
                    Square square = board[file, rank];
                    if (square.IsOccupied)
                    {
                        sb.Append(square.Piece.Symbol);
                    }
                    else
                    {
                        sb.Append(".");
                    }
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            sb.AppendLine("  a b c d e f g h");
            return sb.ToString();
        }
    }
}
