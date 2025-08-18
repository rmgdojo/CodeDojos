namespace RMGChess.Core
{
    public class EnPassantMove : Move
    {
        public Pawn PawnToTakeEnPassant { get; init; }
        public bool TakesPawnEnPassant => PawnToTakeEnPassant is not null;
        public Direction EnPassantDirection { get; init; }

        public static bool CanEnPassant(Pawn pawn, Direction direction)
        {
            return CanEnPassant(pawn, direction, out _);
        }

        public static bool CanEnPassant(Pawn pawn, Direction enPassantDirection, out Pawn pawnToTake)
        {
            if (pawn?.Square?.Board is not null) // guard check, all three must not be null
            {
                Direction trueDirection = enPassantDirection switch
                {
                    Direction.Left => pawn.IsWhite ? Direction.UpLeft : Direction.DownLeft,
                    Direction.Right => pawn.IsWhite ? Direction.UpRight : Direction.DownRight,
                    _ => enPassantDirection
                };

                Board board = pawn.Square.Board;
                Square to = pawn.Square.GetNeighbour(trueDirection);
                Square opponentSquare = to?.GetNeighbour(pawn.IsWhite ? Direction.Down : Direction.Up);

                if (opponentSquare is not null && opponentSquare.IsOccupied && opponentSquare.Piece.IsOpponentOf(pawn) && opponentSquare.Piece is Pawn)
                {
                    Move lastMove = board.Game.LastMoveFor(opponentSquare.Piece.Colour);
                    // may be able to en passant
                    if (lastMove.To == opponentSquare.Position && lastMove.Direction == Direction.Down)
                    {
                        pawnToTake = opponentSquare.Piece as Pawn;
                        return true;
                    }
                }
            }

            pawnToTake = null;
            return false;
        }

        public override string ToString()
        {
            return $"{base.ToString()}.e.p";
        }

        internal override void Execute(Game game)
        {
            Board board = game?.Board;
            // check whether we can take the pawn en passant
            if (board is not null && CanEnPassant(Piece as Pawn, EnPassantDirection, out Pawn pawnToTake))
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
            // direction will be the actual direction of the move (worked out from from -> to)
            // but we need to work out the en passant direction (left or right)
            EnPassantDirection = Direction switch
            {
                Direction.UpLeft => Direction.Left,
                Direction.UpRight => Direction.Right,
                Direction.DownLeft => Direction.Left,
                Direction.DownRight => Direction.Right,
                _ => Direction
            };

            // if this is being cloned as a history item, the square will be null - this is potentially a problem but that's a TODO
            if (piece is not null && piece.Square is not null && CanEnPassant(piece, EnPassantDirection))
            {
                Square square = piece.Square.Board[to];
                Piece pieceToTakeEnPassant = piece.IsWhite ? square?.Down.Piece ?? null : square?.Up.Piece ?? null;
                if (pieceToTakeEnPassant is Pawn) PawnToTakeEnPassant = pieceToTakeEnPassant as Pawn;
            }
        }
    }
}
