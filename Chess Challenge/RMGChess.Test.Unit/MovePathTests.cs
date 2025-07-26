using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class MovePathTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var from = new Position('e', 2);
            var to = new Position('e', 4);
            var path = new MovePath(from, to, Direction.Up);
            Assert.That(path.From, Is.EqualTo(from));
            Assert.That(path.To, Is.EqualTo(to));
            Assert.That(path.Direction, Is.EqualTo(Direction.Up));
        }

        [Test]
        public void ToString_ShouldContainAllSteps()
        {
            var from = new Position('e', 2);
            var to = new Position('e', 4);
            var path = new MovePath(from, to, Direction.Up);
            Assert.That(path.ToString(), Does.Contain("e2"));
            Assert.That(path.ToString(), Does.Contain("e3"));
            Assert.That(path.ToString(), Does.Contain("e4"));
        }
    }
}
