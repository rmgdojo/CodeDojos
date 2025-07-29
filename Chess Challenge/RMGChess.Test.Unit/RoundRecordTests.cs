using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class RoundRecordTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var whiteMove = new MoveRecord(1, "e4", Colour.White);
            var blackMove = new MoveRecord(2, "e5", Colour.Black);
            var roundRecord = new RoundRecord(1, whiteMove, blackMove);
            Assert.That(roundRecord.RoundIndex, Is.EqualTo(1));
            Assert.That(roundRecord.WhiteMove, Is.EqualTo(whiteMove));
            Assert.That(roundRecord.BlackMove, Is.EqualTo(blackMove));
            Assert.That(roundRecord.Moves.Length, Is.EqualTo(2));
        }

        [Test]
        public void ToString_ShouldReturnExpectedFormat()
        {
            var whiteMove = new MoveRecord(1, "e4", Colour.White);
            var blackMove = new MoveRecord(2, "e5", Colour.Black);
            var roundRecord = new RoundRecord(1, whiteMove, blackMove);
            Assert.That(roundRecord.ToString(), Is.EqualTo("e4 e5"));
        }
    }
}
