namespace RMGChess.Core
{
    public class Square
    {
        public bool IsOccupied => Piece != null;
        public Piece Piece { get; private set; }

        internal void PlacePiece(Piece piece)
        {
            Piece = piece;
        }
    }
}
