using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public class Move
    {
        public Piece Piece { get; protected set; }
        public Position From { get; protected set; }
        public Position To { get; protected set; }
        public Direction Direction { get; protected set; }
        public bool TakesPiece => PieceToTake is not null;
        public Piece PieceToTake { get; protected set; }

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

            return (fileDifference, rankDifference) switch
            {
                (2, 1) => Direction.LShaped,
                (1, 2) => Direction.LShaped,
                (0, > 0) => Direction.Up,
                (0, < 0) => Direction.Down,
                (> 0, 0) => Direction.Right,
                (< 0, 0) => Direction.Left,
                (> 0, > 0) => Direction.UpRight,
                (< 0, > 0) => Direction.UpLeft,
                (> 0, < 0) => Direction.DownRight,
                (< 0, < 0) => Direction.DownLeft,
                _ => throw new InvalidOperationException("Invalid move direction")
            };
        }

        protected Move(Piece piece, Position from, Position to, Direction direction, Piece pieceToTake)
        {
            Piece = piece;
            From = from;
            To = to;
            Direction = direction;
            PieceToTake = pieceToTake;
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
