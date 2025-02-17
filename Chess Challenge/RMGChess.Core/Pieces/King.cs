
namespace RMGChess.Core
{
    public class King : Piece
    {
        public override int Value => 10;
        public override int MaxSquares => 1;
        public override MoveType MoveTypes => MoveType.Vertical | MoveType.Horizontal | MoveType.Diagonal | MoveType.Castling;

        public King(Colour colour) : base(colour)
        {
        }
    }
}
