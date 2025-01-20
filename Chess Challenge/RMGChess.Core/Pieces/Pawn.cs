

namespace RMGChess.Core
{
    public class Pawn : Piece
    {
        public override int Value => 1;
        public override int MaxSquares => 1;
        public override MoveType MoveTypes => MoveType.Vertical | MoveType.NotBackwards | MoveType.TakesDiagonally;
        public bool HasMoved { get; private set; }

        public override IEnumerable<Move> GetValidMoves()
        {
            if (Square is null) return new Move[0];

            IList<Move> validMoves = new List<Move>();

            int currentMaxSquares = MaxSquares;
            if (Square.Rank == 2 || Square.Rank == 7)
            {
                currentMaxSquares = 2;
            }

            if (Colour == Colour.White)
            {
                for (int i = 1; i <= currentMaxSquares; i++)
                {
                    if (Square.Rank + i <= 8)
                    {
                        Square square = Square.Up;
                        validMoves.Add(new Move(this, Square, square));
                    }
                }
            }
            else
            {
                for (int i = 1; i <= MaxSquares; i++)
                {

                    if (Square.Rank - i >= 1)
                    {
                        Square square = Square.Down;
                        validMoves.Add(new Move(this, Square, square));
                    }
                }
            }

            return validMoves;
        }

        public Pawn(Colour colour) : base(colour)
        {
        }
    }
}
