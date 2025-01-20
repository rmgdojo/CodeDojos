namespace RMGChess.Core
{
    public class Knight : Piece
    {
        public override int Value => 3;
        public override int MaxSquares => 3;
        public override MoveType MoveTypes => MoveType.LShaped;

        public Knight(Colour colour) : base(colour)
        {
        }
    }
}
