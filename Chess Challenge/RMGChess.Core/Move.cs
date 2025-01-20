using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public class Move
    {
        public Piece Piece { get; init; }
        public Square From { get; init; }
        public Square To { get; init; }

        public Move(Piece piece, Square from, Square to)
        {
            Piece = piece;
            From = from;
            To = to;
        }
    }
}
