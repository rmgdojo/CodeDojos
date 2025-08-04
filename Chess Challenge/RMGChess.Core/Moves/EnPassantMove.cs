namespace RMGChess.Core
{
    public class EnPassantMove : Move
    {
        public static bool CanEnPassant(Pawn pawn, Direction direction)
        {
            return CanEnPassant(pawn, direction, out _);
        }

        public static bool CanEnPassant(Pawn pawn, Direction direction, out Pawn pawnToTake)
        {
            if (pawn?.Square?.Board is not null) // guard check, all three must not be null
            {
                Board board = pawn.Square.Board;

                Square left = pawn.Square.Left?.GetNeighbour(pawn.IsWhite ? Direction.Up : Direction.Down);
                Square right = pawn.Square.Right?.GetNeighbour(pawn.IsWhite ? Direction.Up : Direction.Down);
                //Square leftDestination = left?.GetNeighbour(pawn.IsWhite ? Direction.Up : Direction.Down);
                //Square rightDestination = right?.GetNeighbour(pawn.IsWhite ? Direction.Up : Direction.Down);

                if (direction == Direction.Left && left is not null && left.IsOccupied && left.Piece.IsOpponentOf(pawn) && left.Piece is Pawn)
                {
                    Move lastMove = board.Game.LastMoveFor(left.Piece.Colour);
                    // may be able to en passant left
                    if (lastMove.To == left.Position && lastMove.Direction == Direction.Down)
                    {
                        pawnToTake = left.Piece as Pawn;
                        return true;
                    }
                }

                if (direction == Direction.Right && right is not null && right.IsOccupied && right.Piece.IsOpponentOf(pawn) && right.Piece is Pawn)
                {
                    Move lastMove = board.Game.LastMoveFor(right.Piece.Colour);
                    // may be able to en passant right
                    if (lastMove.To == right.Position && lastMove.Direction == Direction.Down)
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
            // convert directions from regular pawn capture move
            // (up left / down left == left, up right / down right == right)
            if (Direction == Direction.UpLeft || Direction == Direction.DownLeft)
            {
                Direction = Direction.Left;
            }
            else if (Direction == Direction.UpRight || Direction == Direction.DownRight)
            {
                Direction = Direction.Right;
            }
        }
    }
}
