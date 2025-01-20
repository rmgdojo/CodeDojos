namespace RMGChess.Core
{
    public abstract class Piece
    {
        private string _id;

        public Colour Colour { get; init; }
        public virtual int Value => 0;
        public virtual int MaxSquares => Board.MAX_DISTANCE;
        public virtual MoveType MoveTypes => MoveType.None;
        public Square Square { get; set; }
        public char Symbol => GetType().Name.ToUpper()[0];

        public virtual IEnumerable<Move> GetValidMoves()
        {
            if (Square is null) return new Move[0];

            List<Move> validMoves = new();
            foreach (MoveType moveType in Enum.GetValues<MoveType>())
            {
                if (moveType > 0 && MoveTypes.HasFlag(moveType))
                {
                    switch (moveType)
                    {
                        case MoveType.Horizontal:
                            validMoves.AddRange(GetHorizontalMoves());
                            break;
                        case MoveType.Vertical:
                            validMoves.AddRange(GetVerticalMoves());
                            break;
                        case MoveType.Diagonal:
                            validMoves.AddRange(GetDiagonalMoves());
                            break;
                    }
                }
            }

            return validMoves;
        }

        private IList<Move> GetHorizontalMoves()
        {
            IList<Move> validMoves = new List<Move>();

            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.File + i <= 'h')
                {
                    Square square = Square.Right;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.File - i >= 'a')
                {
                    Square square = Square.Left;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            return validMoves;
        }

        private IList<Move> GetVerticalMoves()
        {
            IList<Move> validMoves = new List<Move>();

            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.Rank + i <= 8)
                {
                    Square square = Square.Up;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.Rank - i >= 1)
                {
                    Square square = Square.Down;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            return validMoves;
        }

        private IList<Move> GetDiagonalMoves()
        {
            IList<Move> validMoves = new List<Move>();

            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.File + i <= 'h' && Square.Rank + i <= 8)
                {
                    Square square = Square.UpRight;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.File - i >= 'a' && Square.Rank + i <= 8)
                {
                    Square square = Square.UpLeft;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.File + i <= 'h' && Square.Rank - i >= 1)
                {
                    Square square = Square.DownRight;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            for (int i = 1; i <= MaxSquares; i++)
            {
                if (Square.File - i >= 'a' && Square.Rank - i >= 1)
                {
                    Square square = Square.DownLeft;
                    validMoves.Add(new Move(this, Square, square));
                }
            }
            return validMoves;
        }

        public override string ToString() => _id;

        public Piece(Colour colour)
        {
            Colour = colour;
            _id = $"{(Colour == Colour.White ? "W" : "B")}{GetType().Name[0]}";
        }
    }
}
