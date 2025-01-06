namespace RMGChess.Core
{
    public class Rook : Piece
    {
        public override int Value => 5;
        public override MoveType MoveType => MoveType.Horizontal | MoveType.Vertical;

        public Rook(Colour colour) : base(colour)
        {
        }
    }
}
