
namespace RMGChess.Core
{
    public class Knight : Piece
    {
        public override int Value => 3;
        public override int MaxSquares => 3;
        public override MoveType MoveTypes => MoveType.LShaped;
        public override char Symbol => 'H';

        public override IEnumerable<Move> GetPotentialMoves()
        {
            if (Square is null || Square.Board is null) return new Move[0];
            
            List<Move> validMoves = new();

            int[] fileOffsets = { 2, 2, -2, -2, 1, 1, -1, -1 };
            int[] rankOffsets = { 1, -1, 1, -1, 2, -2, 2, -2 };

            for (int i = 0; i < fileOffsets.Length; i++)
            {
                char newFile = (char)(Square.File + fileOffsets[i]);
                int newRank = Square.Rank + rankOffsets[i];

                if (newFile >= 'a' && newFile <= 'h' && newRank >= 1 && newRank <= 8)
                {
                    Square newSquare = Square.Board[newFile, newRank];
                    if (newSquare != null && (!newSquare.IsOccupied || newSquare.Piece.Colour != this.Colour))
                    {
                        validMoves.Add(new Move(this, Square, newSquare));
                    }
                }
            }

            return validMoves;
        }

        public Knight(Colour colour) : base(colour)
        {
        }
    }
}
