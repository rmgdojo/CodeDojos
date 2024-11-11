namespace RMGChess.Core
{
    public class Square
    {
        public char File { get; }
        public int Rank { get; }

        public bool IsOccupied => Piece != null;
        public Piece Piece { get; private set; }

        internal void PlacePiece(Piece piece)
        {
            Piece = piece;
        }

        public Square(char file, int rank)
        {
            File = file;
            Rank = rank;
        }
    }
}
