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
        public bool TakesPiece => PieceToTake is not null;
        public Piece PieceToTake { get; init; }

        public Move Taking(Piece piece)
        {
            return new Move(Piece, From, To, piece);
        }

        public override string ToString()
        {
            return $"{Piece.Symbol}{From.Position} -> {To.Position}{(TakesPiece ? $"x{PieceToTake.Symbol}{To.Position}" : "")}";
        }

        public Move(Piece piece, Square from, Square to, Piece pieceToTake = null)
        {
            Piece = piece;
            From = from;
            To = to;
            PieceToTake = pieceToTake;
        }
    }
}
