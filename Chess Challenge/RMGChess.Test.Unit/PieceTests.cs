using NUnit.Framework;
using RMGChess.Core;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class PieceTests
    {
        [Test]
        public void TypeNameFromSymbol_ShouldReturnCorrectTypeName()
        {
            Assert.That(Piece.TypeNameFromSymbol('K'), Is.EqualTo("King"));
            Assert.That(Piece.TypeNameFromSymbol('Q'), Is.EqualTo("Queen"));
            Assert.That(Piece.TypeNameFromSymbol('R'), Is.EqualTo("Rook"));
            Assert.That(Piece.TypeNameFromSymbol('B'), Is.EqualTo("Bishop"));
            Assert.That(Piece.TypeNameFromSymbol('N'), Is.EqualTo("Knight"));
            Assert.That(Piece.TypeNameFromSymbol('P'), Is.EqualTo("Pawn"));
        }

        [Test]
        public void SymbolFromType_ShouldReturnCorrectSymbol()
        {
            Assert.That(Piece.SymbolFromTypeName("King"), Is.EqualTo('K'));
            Assert.That(Piece.SymbolFromTypeName("Queen"), Is.EqualTo('Q'));
            Assert.That(Piece.SymbolFromTypeName("Rook"), Is.EqualTo('R'));
            Assert.That(Piece.SymbolFromTypeName("Bishop"), Is.EqualTo('B'));
            Assert.That(Piece.SymbolFromTypeName("Knight"), Is.EqualTo('N'));
            Assert.That(Piece.SymbolFromTypeName("Pawn"), Is.EqualTo('P'));
        }

        [Test]
        public void FromType_ShouldReturnInstanceOfCorrectType()
        {
            Assert.That(Piece.FromTypeName("King", Colour.White), Is.InstanceOf<King>());
            Assert.That(Piece.FromTypeName("Queen", Colour.Black), Is.InstanceOf<Queen>());
            Assert.That(Piece.FromTypeName("Rook", Colour.White), Is.InstanceOf<Rook>());
            Assert.That(Piece.FromTypeName("Bishop", Colour.Black), Is.InstanceOf<Bishop>());
            Assert.That(Piece.FromTypeName("Knight", Colour.White), Is.InstanceOf<Knight>());
            Assert.That(Piece.FromTypeName("Pawn", Colour.Black), Is.InstanceOf<Pawn>());
        }
    }

    [TestFixture]
    public class PawnTests
    {
        [Test]
        public void Pawn_Properties_ShouldBeCorrect()
        {
            var pawn = (Pawn)Piece.FromTypeName("Pawn", Colour.White);
            Assert.That(pawn.Value, Is.EqualTo(1));
            Assert.That(pawn.MaxSquares, Is.EqualTo(1));
            Assert.That(pawn.MoveTypes.HasFlag(MoveType.Vertical), Is.True);
        }
    }

    [TestFixture]
    public class RookTests
    {
        [Test]
        public void Rook_Properties_ShouldBeCorrect()
        {
            var rook = (Rook)Piece.FromTypeName("Rook", Colour.White);
            Assert.That(rook.Value, Is.EqualTo(5));
            Assert.That(rook.MoveTypes.HasFlag(MoveType.Horizontal), Is.True);
            Assert.That(rook.MoveTypes.HasFlag(MoveType.Vertical), Is.True);
        }
    }

    [TestFixture]
    public class KnightTests
    {
        [Test]
        public void Knight_Properties_ShouldBeCorrect()
        {
            var knight = (Knight)Piece.FromTypeName("Knight", Colour.White);
            Assert.That(knight.Value, Is.EqualTo(3));
            Assert.That(knight.MaxSquares, Is.EqualTo(3));
            Assert.That(knight.MoveTypes.HasFlag(MoveType.LShaped), Is.True);
            Assert.That(knight.Symbol, Is.EqualTo('N'));
        }
    }

    [TestFixture]
    public class BishopTests
    {
        [Test]
        public void Bishop_Properties_ShouldBeCorrect()
        {
            var bishop = (Bishop)Piece.FromTypeName("Bishop", Colour.White);
            Assert.That(bishop.Value, Is.EqualTo(3));
            Assert.That(bishop.MoveTypes.HasFlag(MoveType.Diagonal), Is.True);
        }
    }

    [TestFixture]
    public class QueenTests
    {
        [Test]
        public void Queen_Properties_ShouldBeCorrect()
        {
            var queen = (Queen)Piece.FromTypeName("Queen", Colour.White);
            Assert.That(queen.Value, Is.EqualTo(9));
            Assert.That(queen.MoveTypes.HasFlag(MoveType.Vertical), Is.True);
            Assert.That(queen.MoveTypes.HasFlag(MoveType.Horizontal), Is.True);
            Assert.That(queen.MoveTypes.HasFlag(MoveType.Diagonal), Is.True);
        }
    }

    [TestFixture]
    public class KingTests
    {
        [Test]
        public void King_Properties_ShouldBeCorrect()
        {
            var king = (King)Piece.FromTypeName("King", Colour.White);
            Assert.That(king.Value, Is.EqualTo(10));
            Assert.That(king.MaxSquares, Is.EqualTo(1));
            Assert.That(king.MoveTypes.HasFlag(MoveType.Castling), Is.True);
        }
    }
}
