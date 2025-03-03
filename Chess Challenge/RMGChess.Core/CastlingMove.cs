namespace RMGChess.Core
{
    public class CastlingMove : Move
    {
        public CastlingType Type { get; private set; }

        public override void Execute(Board board)
        {
            if (Piece.HasMoved)
            {
                throw new InvalidOperationException("Cannot castle with a king that has moved.");
            }

            if (!KingPathIsEmpty())
            {
                throw new InvalidOperationException("Cannot castle through occupied squares.");
            }

            Move rookMove = GetRookMove(board);
            base.Execute(board);
            rookMove.Execute(board);
        }

        private bool KingPathIsEmpty()
        {
            bool squaresAreEmpty = false;
            Square kingSquare = Piece.Square;
            switch (Type)
            {
                case CastlingType.Kingside:
                    squaresAreEmpty = kingSquare.Right.IsOccupied || kingSquare.Right.Right.IsOccupied;
                    break;

                case CastlingType.Queenside:
                    squaresAreEmpty = kingSquare.Left.IsOccupied || kingSquare.Left.Left.IsOccupied || kingSquare.Left.Left.Left.IsOccupied;
                    break;
            }

            return squaresAreEmpty;
        }

        private Move GetRookMove(Board board)
        {
            string rookSquare = Type switch
            {
                CastlingType.Kingside => Piece.IsWhite ? "h1" : "h8",
                CastlingType.Queenside => Piece.IsWhite ? "a1" : "a8",
                _ => throw new InvalidOperationException("Invalid castling type")
            };

            Piece rook = board[rookSquare].Piece;
            if (rook is null || rook is not Rook || rook.HasMoved)
            {
                throw new InvalidOperationException("Cannot castle with a rook that has moved.");
            }

            // identify position that the rook will move to
            Position rookTo = Type switch
            {
                CastlingType.Kingside => Piece.IsWhite ? new Position('f', 1) : new Position('f', 8),
                CastlingType.Queenside => Piece.IsWhite ? new Position('d', 1) : new Position('d', 8),
                _ => throw new InvalidOperationException("Invalid castling type")
            };
            
            return new Move(rook, rook.Square.Position, rookTo);
        }

        public CastlingMove(King king, CastlingType type)
        {
            Piece = king;
            Type = type;
            From = king.Square.Position;
            To = type switch
            {
                CastlingType.Kingside => king.IsWhite ? new Position('g', 1) : new Position('g', 8),
                CastlingType.Queenside => king.IsWhite ? new Position('c', 1) : new Position('c', 8),
                _ => throw new InvalidOperationException("Invalid castling type")
            };
            Direction = GetDirection(From, To);
        }
    }
}
