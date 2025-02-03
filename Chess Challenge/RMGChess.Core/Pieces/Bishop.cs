namespace RMGChess.Core
{
    public class Bishop : Piece
    {
        public override int Value => 3;
        public override MoveType MoveTypes => MoveType.Diagonal;

        public Bishop(Colour colour) : base(colour)
        {
        }
    }
}
