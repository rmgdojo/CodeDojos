using System;
using System.Collections.Generic;
using System.Linq;
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
        public override string ToString()
        {
            return $"{Piece.Symbol}{From}{(TakesPiece ? $"x{PieceToTake.Symbol}" : "")}{To}"; // always use full form for this
        }

        internal virtual void Execute(Game game)
        {
            Board board = game.Board;
            Piece pieceToRemove = board[From].Piece;
            if (pieceToRemove != Piece)
            {
                throw new InvalidMoveException("Invalid move state: Piece to move does not match the piece on the board.");
            }

            Piece.HasMoved = true;
            board[From].RemovePiece();
            if (TakesPiece)
            {
                Piece taken = board[To].RemovePiece();
                game.HandleCapture(taken);
            }
            board[To].PlacePiece(Piece);
        }

        internal Move Taking(Piece piece)
        {
            return new Move(Piece, From, To, piece);
        }

        protected Direction GetDirection(Position from, Position to)
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

            throw new ChessException("Invalid move direction");
        }

        public Move(Piece piece, Position from, Position to, Piece pieceToTake = null)
        {
            Piece = piece;
            From = from;
            To = to;
            PieceToTake = pieceToTake;
            Direction = GetDirection(from, to);
        }

        protected Move()
        {
        }

        public static implicit operator string(Move move)
        {
            return move.ToString();
        }
    }
}
