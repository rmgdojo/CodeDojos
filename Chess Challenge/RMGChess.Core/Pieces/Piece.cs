namespace RMGChess.Core
{
    public abstract class Piece
    {
        private string _id;

        public Colour Colour { get; init; }
        public virtual int Value => 0;
        public virtual int MaxSquares => Int32.MaxValue;
        public virtual MoveType MoveType => MoveType.None;

        public override string ToString() => _id;

        public Piece(Colour colour)
        {
            Colour = colour;
            _id = $"{(Colour == Colour.White ? "W" : "B")}{GetType().Name[0]}";
        }
    }
}
