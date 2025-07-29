using NUnit.Framework;
using RMGChess.Core;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class AlgebraTests
    {
        [Test]
        public void EncodeAlgebra_PawnMove_ShouldReturnExpectedString()
        {
            var game = new Game();
            var pawn = game.Board["e2"].Piece as Pawn ?? null;
            var move = new Move(pawn, pawn?.Position, new Position('e', 4));
            var encodedMove = Algebra.EncodeAlgebra(move, game.Board);
            Assert.That(encodedMove, Is.EqualTo("e4"));
        }

        [Test]
        public void DecodeAlgebra_PawnMove_ShouldReturnMove()
        {
            var game = new Game();
            var encodedMove = "e4";
            var move = Algebra.DecodeAlgebra(encodedMove, game.Board, Colour.White);
            Assert.That(
                move is not null &&
                move.Piece is Pawn &&
                move.From == "e2" &&
                move.To == "e4"
                );
        }
    }
}
