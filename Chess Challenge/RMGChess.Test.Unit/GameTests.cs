using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void Constructor_ShouldCreateBoard()
        {
            var game = new Game();
            Assert.That(game.Board, Is.Not.Null);
        }
    }
}
