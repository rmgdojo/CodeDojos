namespace RMGChess.Core
{
    public class Queen : Piece
    {
        public override int Value => 9;
        public override MoveType MoveTypes => MoveType.Vertical | MoveType.Horizontal | MoveType.Diagonal;

        public Queen() : base()
        {
        }
    }
}
