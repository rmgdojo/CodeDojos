using System.Collections.ObjectModel;

namespace RMGChess.Core
{
    public class PieceCollection : ReadOnlyCollection<Piece>
    {
        public PieceCollection White => this.Where(p => p.IsWhite).ToPieceCollection();
        public PieceCollection Black => this.Where(p => p.IsBlack).ToPieceCollection();

        public PieceCollection(IEnumerable<Piece> pieces) : base(pieces.ToList()) { }   
    }

    public static class PieceCollectionExtensions
    {
        public static PieceCollection ToPieceCollection(this IEnumerable<Piece> pieces) => new PieceCollection(pieces);
    }
}
