using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var pos = new Position('e', 4);
            Assert.That(pos.File, Is.EqualTo('e'));
            Assert.That(pos.Rank, Is.EqualTo(4));
        }

        [Test]
        public void ImplicitConversionFromString_ShouldWork()
        {
            Position pos = "e4";
            Assert.That(pos.File, Is.EqualTo('e'));
            Assert.That(pos.Rank, Is.EqualTo(4));
        }

        [Test]
        public void ImplicitConversionFromTuple_ShouldWork()
        {
            Position pos = ('d', 5);
            Assert.That(pos.File, Is.EqualTo('d'));
            Assert.That(pos.Rank, Is.EqualTo(5));
        }

        [Test]
        public void ToString_ShouldReturnExpectedFormat()
        {
            var pos = new Position('f', 6);
            Assert.That(pos.ToString(), Is.EqualTo("f6"));
        }
    }
}
