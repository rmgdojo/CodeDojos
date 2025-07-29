using NUnit.Framework;
using RMGChess.Core;
using System;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class MoveTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var piece = Piece.FromType(typeof(Pawn), Colour.White);
            var from = new Position('e', 2);
            var to = new Position('e', 4);
            var move = new Move(piece, from, to);
            Assert.That(move.Piece, Is.EqualTo(piece));
            Assert.That(move.From, Is.EqualTo(from));
            Assert.That(move.To, Is.EqualTo(to));
            Assert.That(move.Direction, Is.EqualTo(Direction.Up));
            Assert.That(move.Path, Is.Not.Null);
        }

        [Test]
        public void ToString_ShouldReturnExpectedFormat()
        {
            var piece = Piece.FromType(typeof(Pawn), Colour.White);
            var from = new Position('e', 2);
            var to = new Position('e', 4);
            var move = new Move(piece, from, to);
            Assert.That(move.ToString(), Does.Contain("e2"));
            Assert.That(move.ToString(), Does.Contain("e4"));
        }
    }

    [TestFixture]
    public class EnPassantMoveTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var pawn = (Pawn)Piece.FromType(typeof(Pawn), Colour.White);
            var from = new Position('e', 5);
            var to = new Position('d', 6);
            var move = new EnPassantMove(pawn, from, to);
            Assert.That(move.Piece, Is.EqualTo(pawn));
            Assert.That(move.From, Is.EqualTo(from));
            Assert.That(move.To, Is.EqualTo(to));
        }
    }

    [TestFixture]
    public class CastlingMoveTests
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _game = new Game();
        }

        [Test]
        public void Constructor_Kingside_ShouldSetProperties()
        {
            var king = _game.Board['e', 1].Piece as King;
            var move = new CastlingMove(king, Side.Kingside);
            Assert.That(move.Piece, Is.EqualTo(king));
            Assert.That(move.Side, Is.EqualTo(Side.Kingside));
        }

        [Test]
        public void Constructor_Queenside_ShouldSetProperties()
        {
            var king = _game.Board['e', 1].Piece as King;
            var move = new CastlingMove(king, Side.Queenside);
            Assert.That(move.Piece, Is.EqualTo(king));
            Assert.That(move.Side, Is.EqualTo(Side.Queenside));
        }
    }
}
