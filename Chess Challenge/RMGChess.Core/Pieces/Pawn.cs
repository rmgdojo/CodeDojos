

namespace RMGChess.Core
{
    public class Pawn : Piece
    {
        public override int Value => 1;
        public override int MaxSquares => 1;
        public override MoveType MoveTypes => MoveType.Vertical | MoveType.NotBackwards | MoveType.TakesDiagonally;
        public bool HasMoved { get; private set; }

        public override IEnumerable<Move> GetPotentialMoves()
        {
            if (Square is null) return new Move[0];

            IList<Move> potentialMoves = new List<Move>();

            Direction direction = IsWhite ? Direction.Up : Direction.Down;
            bool onStartSquare = (Square.Rank == 2 && IsWhite) || (Square.Rank == 7 && IsBlack);
            Square first = Square.GetNeighbour(direction);
            Square second = first?.GetNeighbour(direction);

            if (first is not null) 
            {
                potentialMoves.Add(new Move(this, Square.Position, first.Position));
                if (onStartSquare && second is not null)
                {
                    // could move two squares
                    potentialMoves.Add(new Move(this, Square.Position, second.Position));
                }
            }

            return potentialMoves;
        }

        public Pawn(Colour colour) : base(colour)
        {
        }
    }
}
