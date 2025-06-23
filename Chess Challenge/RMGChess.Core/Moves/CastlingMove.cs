using System.Drawing;
using System.Net.NetworkInformation;

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

        public Side Side { get; private set; }

        internal override void Execute(Game game)
        {
            Board board = game.Board;
            King king = Piece as King;
            Rook rook = GetMovingRook(king, Side) as Rook;

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

            // identify position that the rook will move to
            Position rookTo = Side switch
            {
                Side.Kingside => Piece.IsWhite ? "f1" : "f8",
                Side.Queenside => Piece.IsWhite ? "d1" : "d8",
                _ => throw new ShouldNeverHappenException("Invalid castling side.")
            };

            Move rookMove = new Move(rook, rook.Position, rookTo);

            base.Execute(game);
            rookMove.Execute(game);
        }

        public CastlingMove(King king, Side type)
        {
            Piece = king;
            Side = type;
            From = king.Position;
            To = type switch
            {
                Side.Kingside => king.IsWhite ? "g1" :"g8",
                Side.Queenside => king.IsWhite ? "c1" : "c8",
                _ => throw new ShouldNeverHappenException("Invalid castling side.")
            };
            Direction = GetDirection(From, To);
        }
    }
}
