namespace RMGChess.Core
{
    public class OnReadyToMoveEventArgs : EventArgs
    {
        public Colour ColourToMove { get; }
        public IEnumerable<Move> ValidMoves { get; }
        public OnReadyToMoveEventArgs(Colour colourToMove, IEnumerable<Move> validMoves)
        {
            ColourToMove = colourToMove;
            ValidMoves = validMoves;
        }
    }

    public class OnAfterMoveEventArgs : EventArgs
    {
        public Colour Colour { get; }
        public Move Move { get; }
        public OnAfterMoveEventArgs(Colour colourThatMoved, Move move)
        {
            Colour = colourThatMoved;
            Move = move;
        }
    }

    public class OnPiecePromotedEventArgs : EventArgs
    {
        public Piece OriginalPiece { get; }
        public Piece PromotedPiece { get; }
        public OnPiecePromotedEventArgs(Piece originalPiece, Piece promotedPiece)
        {
            OriginalPiece = originalPiece;
            PromotedPiece = promotedPiece;
        }
    }

    public class OnGameEndedEventArgs : EventArgs
    {
        public GameEndReason Reason { get; }
        public OnGameEndedEventArgs(GameEndReason reason)
        {
            Reason = reason;
        }
    }

    public class OnCheckEventArgs : EventArgs
    {
        public Colour Colour { get; }
        public IEnumerable<Piece> CheckingPieces { get; }
        public OnCheckEventArgs(Colour colour, IEnumerable<Piece> checkingPieces)
        {
            Colour = colour;
            CheckingPieces = checkingPieces;
        }
    }


}
