using NUnit.Framework;
using RMGChess.Core;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void Constructor_ShouldCreateSquares()
        {
            var game = new Game();
            var board = game.Board;
            Assert.That(board["e2"], Is.Not.Null);
            Assert.That(board['a', 1], Is.Not.Null);
            Assert.That(board['h', 8], Is.Not.Null);
        }

        [Test]
        public void GetAllPiecesThatCanMoveTo_ShouldReturnCollection()
        {
            var game = new Game();
            var board = game.Board;
            var position = new Position('e', 4);
            var pieces = board.GetAllPiecesThatCanMoveTo(position);
            Assert.That(pieces, Is.Not.Null);
            Assert.That(pieces.Count(), Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void GetValidMovesForAllPieces_ShouldReturnMoves()
        {
            var game = new Game();
            var board = game.Board;
            var moves = board.GetValidMovesForAllPieces();
            Assert.That(moves, Is.Not.Null);
            Assert.That(moves.Count(), Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void GetValidMoves_ShouldReturnMovesForPiece()
        {
            var game = new Game();
            var board = game.Board;
            var piece = game.PiecesInPlay.First();
            var moves = board.GetValidMoves(piece);
            Assert.That(moves, Is.Not.Null);
            Assert.That(moves.Count(), Is.GreaterThanOrEqualTo(0));
        }
    }
}
