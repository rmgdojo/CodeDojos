namespace RMGChess.Core
{
    public abstract class Piece
    {
        public Colour Colour { get; init; }
        public virtual int Value => 0;
        public virtual int MaxSquares => Board.MAX_DISTANCE;
        public virtual MoveType MoveTypes => MoveType.None;
        public Square Square { get; set; }
        public virtual char Symbol => GetType().Name.ToUpper()[0];

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

        private IList<Move> GetVerticalMoves()
        {
            IList<Move> validMoves = new List<Move>();

            AddMoves(Square, NeighbourDirection.Up, validMoves);
            AddMoves(Square, NeighbourDirection.Down, validMoves);

            return validMoves;
        }

        private IList<Move> GetHorizontalMoves()
        {
            IList<Move> validMoves = new List<Move>();

            AddMoves(Square, NeighbourDirection.Left, validMoves);
            AddMoves(Square, NeighbourDirection.Right, validMoves);

            return validMoves;
        }

        private IList<Move> GetDiagonalMoves()
        {
            IList<Move> validMoves = new List<Move>();

            AddMoves(Square, NeighbourDirection.UpLeft, validMoves);
            AddMoves(Square, NeighbourDirection.UpRight, validMoves);
            AddMoves(Square, NeighbourDirection.DownLeft, validMoves);
            AddMoves(Square, NeighbourDirection.DownRight, validMoves);

            return validMoves;
        }

        protected void AddMoves(Square square, NeighbourDirection direction, IList<Move> validMoves, int movesSoFar = 0)
        {
            if (square != null)
            {
                Square nextSquare = square.GetNeighbour(direction);
                if (nextSquare != null && movesSoFar++ < MaxSquares)
                {
                    validMoves.Add(new Move(this, square, nextSquare));
                    AddMoves(nextSquare, direction, validMoves, movesSoFar);
                }
            }
        }

        public override string ToString() => $"{(Colour == Colour.White ? "W" : "B")}{GetType().Name[0]}";

        public Piece()
        {
        }
    }
}
