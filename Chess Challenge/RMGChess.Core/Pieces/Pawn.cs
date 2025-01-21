

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

            IList<Move> validMoves = new List<Move>();

            if (Colour == Colour.White) AddMoves(Square, Direction.Up, validMoves);
            if (Colour == Colour.Black) AddMoves(Square, Direction.Down, validMoves);

            return validMoves;
        }

        public Pawn() : base()
        {
        }
    }
}
