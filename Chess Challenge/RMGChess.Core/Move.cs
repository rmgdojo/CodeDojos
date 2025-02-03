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
        public Position From { get; init; }
        public Position To { get; init; }
        public Direction Direction { get; init; }
        public bool TakesPiece => PieceToTake is not null;
        public Piece PieceToTake { get; init; }

        public Move Taking(Piece piece)
        {
            return new Move(Piece, From, To, piece);
        }

        public override string ToString()
        {
            return $"{Piece.Symbol}{From} -> {To}{(TakesPiece ? $"x{PieceToTake.Symbol}{To}" : "")}";
        }

        private Direction GetDirection(Position from, Position to)
        {
            int fileDifference = to.File - from.File;
            int rankDifference = to.Rank - from.Rank;

            if (fileDifference == 0 && rankDifference > 0) return Direction.Up;
            if (fileDifference == 0 && rankDifference < 0) return Direction.Down;
            if (rankDifference == 0 && fileDifference > 0) return Direction.Right;
            if (rankDifference == 0 && fileDifference < 0) return Direction.Left;
            if (fileDifference > 0 && rankDifference > 0) return Direction.UpRight;
            if (fileDifference < 0 && rankDifference > 0) return Direction.UpLeft;
            if (fileDifference > 0 && rankDifference < 0) return Direction.DownRight;
            if (fileDifference < 0 && rankDifference < 0) return Direction.DownLeft;
            if (fileDifference == 2 && rankDifference == 1) return Direction.LShaped;
            if (fileDifference == 1 && rankDifference == 2) return Direction.LShaped;

            throw new InvalidOperationException("Invalid move direction");
        }

        public Move(Piece piece, Square from, Square to, Piece pieceToTake = null)
            : this(piece, from.Position, to.Position, pieceToTake)  
        {
        }

        public Move(Piece piece, Position from, Position to, Piece pieceToTake = null)
        {
            Piece = piece;
            From = from;
            To = to;
            PieceToTake = pieceToTake;
            Direction = GetDirection(from, to);
        }
    }
}
