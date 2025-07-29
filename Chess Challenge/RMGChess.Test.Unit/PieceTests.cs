using NUnit.Framework;
using RMGChess.Core;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class PieceTests
    {
        [Test]
        public void TypeFromSymbol_ShouldReturnCorrectType()
        {
            Assert.That(Piece.TypeFromSymbol('K'), Is.EqualTo(typeof(King)));
            Assert.That(Piece.TypeFromSymbol('Q'), Is.EqualTo(typeof(Queen)));
            Assert.That(Piece.TypeFromSymbol('R'), Is.EqualTo(typeof(Rook)));
            Assert.That(Piece.TypeFromSymbol('B'), Is.EqualTo(typeof(Bishop)));
            Assert.That(Piece.TypeFromSymbol('N'), Is.EqualTo(typeof(Knight)));
            Assert.That(Piece.TypeFromSymbol('P'), Is.EqualTo(typeof(Pawn)));
        }

        [Test]
        public void SymbolFromType_ShouldReturnCorrectSymbol()
        {
            Assert.That(Piece.SymbolFromType(typeof(King)), Is.EqualTo('K'));
            Assert.That(Piece.SymbolFromType(typeof(Queen)), Is.EqualTo('Q'));
            Assert.That(Piece.SymbolFromType(typeof(Rook)), Is.EqualTo('R'));
            Assert.That(Piece.SymbolFromType(typeof(Bishop)), Is.EqualTo('B'));
            Assert.That(Piece.SymbolFromType(typeof(Knight)), Is.EqualTo('N'));
            Assert.That(Piece.SymbolFromType(typeof(Pawn)), Is.EqualTo('P'));
        }

        [Test]
        public void FromType_ShouldReturnInstanceOfCorrectType()
        {
            Assert.That(Piece.FromType(typeof(King), Colour.White), Is.InstanceOf<King>());
            Assert.That(Piece.FromType(typeof(Queen), Colour.Black), Is.InstanceOf<Queen>());
            Assert.That(Piece.FromType(typeof(Rook), Colour.White), Is.InstanceOf<Rook>());
            Assert.That(Piece.FromType(typeof(Bishop), Colour.Black), Is.InstanceOf<Bishop>());
            Assert.That(Piece.FromType(typeof(Knight), Colour.White), Is.InstanceOf<Knight>());
            Assert.That(Piece.FromType(typeof(Pawn), Colour.Black), Is.InstanceOf<Pawn>());
        }
    }

    [TestFixture]
    public class PawnTests
    {
        [Test]
        public void Pawn_Properties_ShouldBeCorrect()
        {
            var pawn = (Pawn)Piece.FromType(typeof(Pawn), Colour.White);
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
            var rook = (Rook)Piece.FromType(typeof(Rook), Colour.White);
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
            var knight = (Knight)Piece.FromType(typeof(Knight), Colour.White);
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
            var bishop = (Bishop)Piece.FromType(typeof(Bishop), Colour.White);
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
            var queen = (Queen)Piece.FromType(typeof(Queen), Colour.White);
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
            var king = (King)Piece.FromType(typeof(King), Colour.White);
            Assert.That(king.Value, Is.EqualTo(10));
            Assert.That(king.MaxSquares, Is.EqualTo(1));
            Assert.That(king.MoveTypes.HasFlag(MoveType.Castling), Is.True);
        }
    }
}
