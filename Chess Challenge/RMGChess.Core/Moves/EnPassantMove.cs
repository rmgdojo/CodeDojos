namespace RMGChess.Core
{
    public class EnPassantMove : Move
    {
        public static bool CanEnPassant(Pawn pawn, Direction direction, out Pawn pawnToTake)
        {
            if (pawn?.Square?.Board is not null) // guard check, all three must not be null
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
            }

            pawnToTake = null;
            return false;
        }

        internal override void Execute(Game game)
        {
            Board board = game?.Board;
            // check whether we can take the pawn en passant
            if (board is not null && CanEnPassant(Piece as Pawn, Direction, out Pawn pawnToTake))
            {
                Colour opponentColour = Piece.IsWhite ? Colour.Black : Colour.White;
                board[pawnToTake.Position].RemovePiece();
                game.HandleCapture(pawnToTake);

                base.Execute(game);
                return;
            }
                
            throw new InvalidMoveException("Invalid en passant move.");
        }

        internal override Move Clone(Piece clonedPiece, Piece clonedPieceToTake)
        {
            return new EnPassantMove(clonedPiece as Pawn, From, To);
        }

        public EnPassantMove(Pawn piece, Position from, Position to) : base(piece, from, to)
        {
        }
    }
}
