namespace RMGChess.Core
{
    public class CastlingMove : Move
    {
        public static bool CanCastle(King king, Side side)
        {
            Rook rook = GetMovingRook(king, side);

            if (king is null || rook is null || king.HasMoved || rook.HasMoved)
            {
                return false;
            }

            bool squareIsOccupied = false;
            Square kingSquare = king.Square;
            switch (side)
            {
                case Side.Kingside:
                    squareIsOccupied = kingSquare.Right.IsOccupied || kingSquare.Right.Right.IsOccupied;
                    break;

                case Side.Queenside:
                    squareIsOccupied = kingSquare.Left.IsOccupied || kingSquare.Left.Left.IsOccupied || kingSquare.Left.Left.Left.IsOccupied;
                    break;
            }

            if (squareIsOccupied)
            {
                return false;
            }

            return true;
        }

        private static Rook GetMovingRook(King king, Side side)
        {
            string rookSquare = side switch
            {
                Side.Kingside => king.IsWhite ? "h1" : "h8",
                Side.Queenside => king.IsWhite ? "a1" : "a8",
                _ => throw new ShouldNeverHappenException("Invalid castling side.")
            };

            return king.Square.Board[rookSquare].Piece as Rook;
        }

        public Move RookMove { get; private set; }
        public Side Side { get; private set; }

        public override string ToString()
        {
            return Side switch
            {
                Side.Kingside => $"O-O",
                Side.Queenside => $"O-O-O",
                _ => throw new ShouldNeverHappenException("Invalid castling side.")
            };
        }

        internal override void Execute(Game game)
        {
            Board board = game.Board;
            King king = Piece as King;
            Rook rook = RookMove.Piece as Rook;

            if (Piece.HasMoved)
            {
                throw new InvalidMoveException("Cannot castle with a king that has moved.");
            }

            if (!CanCastle(king, Side))
            {
                throw new InvalidMoveException("Cannot castle through occupied squares.");
            }

            if (rook is null || rook.HasMoved)
            {
                throw new InvalidMoveException("Cannot castle with a rook that has moved.");
            }

            base.Execute(game, RookMove); // execute the king's move first, then the rook's move
        }

        internal override Move Clone(Piece clonedPiece, Piece clonedPieceToTake)
        {
            King clonedKing = clonedPiece as King;
            Rook clonedRook = GetMovingRook(clonedKing, Side);

            CastlingMove clonedMove = (CastlingMove)MemberwiseClone();
            clonedMove.Piece = clonedPiece;
            clonedMove.RookMove = RookMove.Clone(clonedRook, clonedPieceToTake);
            
            return clonedMove;
        }

        private Position SetupMove(King king, Side type)
        {
            Piece = king;
            Side = type;
            From = king.Position;

            (To, Position rookTo) = type switch
            {
                Side.Kingside => king.IsWhite ? ("g1", "f1") : ("g8", "f8"),
                Side.Queenside => king.IsWhite ? ("c1", "d1") : ("c8", "d8"),
                _ => throw new ShouldNeverHappenException("Invalid castling side.")
            };

            Direction = GetDirection(From, To);

            return rookTo;
        }

        public CastlingMove(King king, Side type)
        {
            Position rookTo = SetupMove(king, type);

            Rook rook = GetMovingRook(king, Side) as Rook;
            RookMove = new Move(rook, rook.Position, rookTo);
            Path = new MovePath(From, To, Direction);
        }
    }
}
