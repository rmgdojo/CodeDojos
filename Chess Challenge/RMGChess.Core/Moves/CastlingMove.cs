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

            if (KingPathIsBlocked(king, side))
            {
                return false;
            }

            return true;
        }

        private static bool KingPathIsBlocked(King king, Side side)
        {
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

            return squareIsOccupied;
        }

        private static Rook GetMovingRook(King king, Side side)
        {
            string rookSquare = side switch
            {
                Side.Kingside => king.IsWhite ? "h1" : "h8",
                Side.Queenside => king.IsWhite ? "a1" : "a8",
                _ => throw new InvalidOperationException("Invalid castling type")
            };

            return king.Square.Board[rookSquare].Piece as Rook;
        }

        public Side Side { get; private set; }

        public override void Execute(Game game)
        {
            Board board = game.Board;
            King king = Piece as King;
            Rook rook = GetMovingRook(king, Side) as Rook;

            if (Piece.HasMoved)
            {
                throw new InvalidOperationException("Cannot castle with a king that has moved.");
            }

            if (!CanCastle(king, Side))
            {
                throw new InvalidOperationException("Cannot castle through occupied squares.");
            }

            Move rookMove = GetRookMove(rook);
            base.Execute(game);
            rookMove.Execute(game);
        }

        private Move GetRookMove(Rook rook)
        {
            if (rook is null || rook.HasMoved)
            {
                throw new InvalidOperationException("Cannot castle with a rook that has moved.");
            }

            // identify position that the rook will move to
            Position rookTo = Side switch
            {
                Side.Kingside => Piece.IsWhite ? new Position('f', 1) : new Position('f', 8),
                Side.Queenside => Piece.IsWhite ? new Position('d', 1) : new Position('d', 8),
                _ => throw new InvalidOperationException("Invalid castling type")
            };
            
            return new Move(rook, rook.Position, rookTo);
        }

        public CastlingMove(King king, Side type)
        {
            Piece = king;
            Side = type;
            From = king.Position;
            To = type switch
            {
                Side.Kingside => king.IsWhite ? new Position('g', 1) : new Position('g', 8),
                Side.Queenside => king.IsWhite ? new Position('c', 1) : new Position('c', 8),
                _ => throw new InvalidOperationException("Invalid castling type")
            };
            Direction = GetDirection(From, To);
        }
    }
}
