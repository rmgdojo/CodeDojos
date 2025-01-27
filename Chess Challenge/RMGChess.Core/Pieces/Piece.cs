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

        public virtual IEnumerable<Move> GetPotentialMoves()
        {
            if (Square is null) return new Move[0];

            List<Move> potentialMoves = new();
            foreach (MoveType moveType in Enum.GetValues<MoveType>())
            {
                if (moveType > 0 && MoveTypes.HasFlag(moveType))
                {
                    switch (moveType)
                    {
                        case MoveType.Horizontal:
                            potentialMoves.AddRange(GetHorizontalMoves());
                            break;
                        case MoveType.Vertical:
                            potentialMoves.AddRange(GetVerticalMoves());
                            break;
                        case MoveType.Diagonal:
                            potentialMoves.AddRange(GetDiagonalMoves());
                            break;
                    }
                }
            }

            return potentialMoves;
        }

        private IList<Move> GetVerticalMoves()
        {
            IList<Move> potentialMoves = new List<Move>();

            AddMoves(Square, Direction.Up, potentialMoves);
            AddMoves(Square, Direction.Down, potentialMoves);

            return potentialMoves;
        }

        private IList<Move> GetHorizontalMoves()
        {
            IList<Move> potentialMoves = new List<Move>();

            AddMoves(Square, Direction.Left, potentialMoves);
            AddMoves(Square, Direction.Right, potentialMoves);

            return potentialMoves;
        }

        private IList<Move> GetDiagonalMoves()
        {
            IList<Move> potentialMoves = new List<Move>();

            AddMoves(Square, Direction.UpLeft, potentialMoves);
            AddMoves(Square, Direction.UpRight, potentialMoves);
            AddMoves(Square, Direction.DownLeft, potentialMoves);
            AddMoves(Square, Direction.DownRight, potentialMoves);

            return potentialMoves;
        }

        protected void AddMoves(Square square, Direction direction, IList<Move> potentialMoves, int movesSoFar = 0)
        {
            if (square != null)
            {
                Square nextSquare = square.GetNeighbour(direction);
                if (nextSquare != null && movesSoFar++ < MaxSquares)
                {
                    potentialMoves.Add(new Move(this, square, nextSquare));
                    AddMoves(nextSquare, direction, potentialMoves, movesSoFar);
                }
            }
        }

        public override string ToString() => $"{(Colour == Colour.White ? "W" : "B")}{Symbol}";

        public Piece()
        {
        }
    }
}
