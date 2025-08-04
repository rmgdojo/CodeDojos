using NUnit.Framework;
using RMGChess.Core;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class PieceCollectionTests
    {
        [Test]
        public void OfColour_ShouldReturnCorrectPieces()
        {
            var pieces = new[]
            {
                Piece.FromTypeName("Pawn", Colour.White),
                Piece.FromTypeName("Knight", Colour.Black),
                Piece.FromTypeName("Rook", Colour.White)
            };
            var collection = new PieceCollection(pieces);
            var whitePieces = collection.OfColour(Colour.White);
            Assert.That(whitePieces.All(p => p.Colour == Colour.White), Is.True);
            var blackPieces = collection.OfColour(Colour.Black);
            Assert.That(blackPieces.All(p => p.Colour == Colour.Black), Is.True);
        }

        [Test]
        public void OfSameTypeAs_ShouldReturnCorrectPieces()
        {
            var pieces = new[]
            {
                Piece.FromTypeName("Pawn", Colour.White),
                Piece.FromTypeName("Pawn", Colour.Black),
                Piece.FromTypeName("Knight", Colour.White)
            };
            var collection = new PieceCollection(pieces);
            var pawns = collection.OfSameTypeAs(pieces[0]);
            Assert.That(pawns.All(p => p is Pawn), Is.True);
        }
    }
}
