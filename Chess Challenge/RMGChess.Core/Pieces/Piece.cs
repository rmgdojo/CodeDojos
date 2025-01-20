namespace RMGChess.Core
{
    public abstract class Piece
    {
        private string _id;

        public Colour Colour { get; init; }
        public virtual int Value => 0;
        public virtual int MaxSquares => Int32.MaxValue;
        public virtual MoveType MoveTypes => MoveType.None;
        public Square Square { get; set; }

        public virtual IEnumerable<Move> GetValidMoves()
        {
            List<Move> validMoves = new();

            foreach(MoveType moveType in Enum.GetValues<MoveType>())
            {
                if (MoveTypes.HasFlag(moveType))
                {
                    switch (moveType)
                    {
                        case MoveType.Horizontal:
                            validMoves.AddRange(GetHorizontalMoves(validMoves));
                            break;
                        case MoveType.Vertical:
                            validMoves.AddRange(GetVerticalMoves(validMoves));
                            break;
                        case MoveType.Diagonal:
                            validMoves.AddRange(GetDiagonalMoves(validMoves));
                            break;
                    }
                }
            }

            return validMoves;
        }

        private IEnumerable<Move> GetHorizontalMoves(List<Move> validMoves)
        {
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

        protected virtual IEnumerable<Move> GetVerticalMoves(List<Move> validMoves)
        {
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

        private IEnumerable<Move> GetDiagonalMoves(List<Move> validMoves)
        {
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
