namespace RMGChess.Core
{
    public enum Colour
    {
        White,
        Black
    }

    public abstract class Piece
    {
        private string _id;

        public Colour Colour { get; init; }
        public virtual int Value => 0;

        public override string ToString() => _id;

        public Piece(Colour colour)
        {
            Colour = colour;
            _id = $"{(Colour == Colour.White ? "W" : "B")}{GetType().Name[0]}";
        }
    }

    public class King : Piece
    {
        public override int Value => Int32.MaxValue;

        public King(Colour colour) : base(colour)
        {
        }
    }

    public class Queen : Piece
    {
        public override int Value => 9;

        public Queen(Colour colour) : base(colour)
        {
        }
    }

    public class Rook : Piece
    {
        public override int Value => 5;

        public Rook(Colour colour) : base(colour)
        {
        }
    }

    public class Bishop : Piece
    {
        public override int Value => 3;

        public Bishop(Colour colour) : base(colour)
        {
        }
    }

    public class Knight : Piece
    {
        public override int Value => 3;

        public Knight(Colour colour) : base(colour)
        {
        }
    }

    public class Pawn : Piece
    {
        public override int Value => 1;

        public Pawn(Colour colour) : base(colour)
        {
        }
    }
}
