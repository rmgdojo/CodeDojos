using System.Collections.ObjectModel;

namespace RMGChess.Core
{
    public class PieceCollection : ReadOnlyCollection<Piece>
    {
        public PieceCollection White => this.All(p => p.IsWhite) ? this : this.Where(p => p.IsWhite).ToPieceCollection();
        public PieceCollection Black => this.All(p => p.IsBlack) ? this : this.Where(p => p.IsBlack).ToPieceCollection();
        public PieceCollection OfColour(Colour colour) => this.All(p => p.Colour == colour) ? this : this.Where(p => p.Colour == colour).ToPieceCollection();
        public PieceCollection OfSameTypeAs(Piece piece) => this.All(p => p.GetType() == piece.GetType()) ? this : this.Where(p => p.GetType() == piece.GetType()).ToPieceCollection();

        public PieceCollection(IEnumerable<Piece> pieces) : base(pieces.ToList()) { }   
    }

    public static class PieceCollectionExtensions
    {
        public static PieceCollection ToPieceCollection(this IEnumerable<Piece> pieces) => new PieceCollection(pieces);
    }
}
