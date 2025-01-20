
namespace RMGChess.Core
{
    public class Pawn : Piece
    {
        public override int Value => 1;
        public override int MaxSquares => 1;
        public override MoveType MoveTypes => MoveType.Vertical | MoveType.NotBackwards | MoveType.TakesDiagonally;
        public bool HasMoved { get; private set; }

        public Pawn(Colour colour) : base(colour)
        {
        }
    }
}
