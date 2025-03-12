namespace RMGChess.Core
{
    public class EnPassantMove : Move
    {
        public static bool CanEnPassant(Pawn pawn, Direction direction, out Pawn pawnToTake)
        {
            Board board = pawn.Square.Board;

            Square left = pawn.Square.Left;
            Square right = pawn.Square.Right;
            Square leftDestination = left?.GetNeighbour(pawn.IsWhite ? Direction.Up : Direction.Down);
            Square rightDestination = right?.GetNeighbour(pawn.IsWhite ? Direction.Up : Direction.Down);

            if (left is not null && left.IsOccupied && left.Piece.IsOpponentOf(pawn) && left.Piece is Pawn)
            {
                // may be able to en passant left
                if (leftDestination is not null && board.Game.LastMoveFor(left.Piece.Colour).To == leftDestination.Position)
                {
                    pawnToTake = left.Piece as Pawn;
                    return true;
                }
            }

            if (right is not null && right.IsOccupied && right.Piece.IsOpponentOf(pawn) && right.Piece is Pawn)
            {
                // may be able to en passant right
                if (rightDestination is not null && board.Game.LastMoveFor(right.Piece.Colour).To == rightDestination.Position)
                {
                    pawnToTake = right.Piece as Pawn;
                    return true;
                }
            }

            pawnToTake = null;
            return false;
        }

        public override void Execute(Game game)
        {
            Board board = game.Board;
            // check whether we can take the pawn en passant
            if (CanEnPassant(Piece as Pawn, Direction, out Pawn pawnToTake))
            {
                Colour opponentColour = Piece.IsWhite ? Colour.Black : Colour.White;
                if (game is not null)
                {
                    if (game.LastMoveFor(opponentColour).To == pawnToTake.Position)
                    {
                        board[pawnToTake.Position].RemovePiece();
                        game.HandleCapture(pawnToTake);
                        
                        base.Execute(game);
                        return;
                    }
                }
            }
                
            throw new InvalidOperationException("Invalid en passant move.");
        }

        public EnPassantMove(Piece piece, Position from, Position to) : base(piece, from, to)
        {
        }
    }
}
