namespace RMGChess.Core
{
    public class Square
    {
        private Board _board;

        public char File { get; }
        public int Rank { get; }
        public Position Position => $"{File}{Rank}";
        public bool IsEdgeSquare => File == 'a' || File == 'h' || Rank == 1 || Rank == 8;

        public Square Left => File == 'a' ? null : _board[(char)(File - 1), Rank];
        public Square Right => File == 'h' ? null : _board[(char)(File + 1), Rank];
        public Square Up => Rank == 8 ? null : _board[File, Rank + 1];
        public Square Down => Rank == 1 ? null : _board[File, Rank - 1];
        public Square UpLeft => Rank == 8 || File == 'a' ? null : _board[(char)(File - 1), Rank + 1];
        public Square UpRight => Rank == 8 || File == 'h' ? null : _board[(char)(File + 1), Rank + 1];
        public Square DownLeft => Rank == 1 || File == 'a' ? null : _board[(char)(File - 1), Rank - 1];
        public Square DownRight => Rank == 1 || File == 'h' ? null : _board[(char)(File + 1), Rank - 1];

        public bool IsOccupied => Piece != null;
        public Piece Piece { get; private set; }

        public Board Board => _board;

        public Square GetNeighbour(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Up,
                Direction.Down => Down,
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.UpLeft => UpLeft,
                Direction.UpRight => UpRight,
                Direction.DownLeft => DownLeft,
                Direction.DownRight => DownRight,
                _ => null,
            };
        }

        public Piece PlacePiece(Piece piece, bool setOrigin = false)
        {
            Piece = piece;
            piece.Square = this;
            if (setOrigin) piece.Origin = Position;
            return piece;
        }

        public Piece RemovePiece()
        {
            Piece thisPiece = Piece;
            if (thisPiece is not null)
            {
                thisPiece.Square = null;
                Piece = null;
            }

            return thisPiece;
        }

        public override string ToString()
        {
            return $"{Position.ToString()}{(IsOccupied ? " " + Piece.ToString() : "")}";
        }

        public Square(Board board, char file, int rank)
        {
            _board = board;
            File = file;
            Rank = rank;
        }
    }
}
