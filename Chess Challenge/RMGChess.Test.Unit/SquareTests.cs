using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class SquareTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var game = new Game();
            var board = game.Board;
            var square = board['e', 4];
            Assert.That(square.File, Is.EqualTo('e'));
            Assert.That(square.Rank, Is.EqualTo(4));
            Assert.That(square.Board, Is.EqualTo(board));
        }
    }
}
