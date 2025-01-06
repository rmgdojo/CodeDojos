namespace RMGChess.Core
{
    public class King : Piece
    {
        public override int Value => Int32.MaxValue;
        public override int MaxSquares => 1;
        public override MoveType MoveType => MoveType.Vertical | MoveType.Horizontal | MoveType.Diagonal | MoveType.Castling;
        public bool HasCastled { get; private set; }

        public King(Colour colour) : base(colour)
        {
        }
    }
}
