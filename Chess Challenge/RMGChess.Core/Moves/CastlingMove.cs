namespace RMGChess.Core
{
    public class CastlingMove : Move
    {
        public Move RookMove { get; init; }

        public override string ToString()
        {
            return $"{Piece.Symbol}{From} -> {To} (Castling)";
        }

        public CastlingMove(King piece, Type type) : base(piece, null, null, Direction.Castling, null)
        {
            From = piece.Square?.Position ?? null;
            To = (type, piece.Colour) switch
            {
                (Type.Kingside, Colour.White) => "g1",
                (Type.Kingside, Colour.Black) => "g8",
                (Type.Queenside, Colour.White) => "c1",
                (Type.Queenside, Colour.Black) => "c8",
                _ => null
            }; 
            
            if (From is null || (From != "e1" && From != "e8"))
            {
                throw new InvalidOperationException("King must not have moved to castle.");
            }

            Board board = piece.Square?.Board ?? throw new InvalidOperationException("King must be on the board to castle.");
            Piece rook = (type, piece.Colour) switch
            {
                (Type.Kingside, Colour.White) => board["h1"].Piece,
                (Type.Kingside, Colour.Black) => board["h8"].Piece,
                (Type.Queenside, Colour.White) => board["a1"].Piece,
                (Type.Queenside, Colour.Black) => board["a8"].Piece,
                _ => null
            };
            if (rook is null)
            {
                throw new InvalidOperationException("Rook has moved, castling is not allowed.");
            }

            Position rookTo = (type, piece.Colour) switch
            {
                (Type.Kingside, Colour.White) => "f1",
                (Type.Kingside, Colour.Black) => "f8",
                (Type.Queenside, Colour.White) => "d1",
                (Type.Queenside, Colour.Black) => "d8",
                _ => null
            };

            RookMove = new Move(rook, rook.Square.Position, rookTo, null);
        }
        
        public enum Type
        {
            Kingside,
            Queenside
        }
    }
}
