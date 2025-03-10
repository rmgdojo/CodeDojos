namespace RMGChess.Core
{
    public class EnPassantMove : Move
    {
        public override void Execute(Game game)
        {
            Board board = game.Board;
            // check whether we can take the pawn en passant
            if (Piece is Pawn && !board[To].IsOccupied)
            {
                Piece pawnToTake = Piece.IsWhite ? board[To].Down.Piece : board[To].Up.Piece;
                if (pawnToTake is Pawn)
                {
                    Colour opponentColour = Piece.IsWhite ? Colour.Black : Colour.White;
                    if (game is not null)
                    {
                        if (game.LastMoveFor(opponentColour).To == pawnToTake.Square.Position)
                        {
                            board[pawnToTake.Square.Position].RemovePiece();
                            base.Execute(game);
                            return;
                        }
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
