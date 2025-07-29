using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class MoveRecordTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var moveRecord = new MoveRecord(1, "e4", Colour.White);
            Assert.That(moveRecord.MoveIndex, Is.EqualTo(1));
            Assert.That(moveRecord.MoveAsAlgebra, Is.EqualTo("e4"));
            Assert.That(moveRecord.WhoseTurn, Is.EqualTo(Colour.White));
            Assert.That(moveRecord.RoundIndex, Is.EqualTo(1f));
            Assert.That(moveRecord.IsWhite, Is.True);
            Assert.That(moveRecord.IsBlack, Is.False);
        }

        [Test]
        public void ToString_ShouldReturnExpectedFormat()
        {
            var moveRecord = new MoveRecord(2, "e5", Colour.Black);
            Assert.That(moveRecord.ToString(), Does.Contain("Black"));
            Assert.That(moveRecord.ToString(), Does.Contain("e5"));
        }
    }
}
