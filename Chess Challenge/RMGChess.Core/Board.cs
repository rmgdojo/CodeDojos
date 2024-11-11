namespace RMGChess.Core
{
    public class Board
    {
        private Square[,] _squares;

        public Square this[char file, int rank]
        {
            get
            {
                file = char.ToLower(file);
                if (file < 'a' || file > 'h' || rank < 1 || rank > 8)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _squares[file - 'a', rank - 1];
            }
        }

        public Board()
        {
            _squares = new Square[8, 8];
            for (char file = 'a'; file <= 'h'; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    _squares[file, rank] = new Square(file, rank);
                }
            }
        }
    }
}
