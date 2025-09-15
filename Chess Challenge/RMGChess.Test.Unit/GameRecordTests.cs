using NUnit.Framework;
using RMGChess.Core;
using System;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class GameRecordTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            var moves = new[] { "e4", "e5", "Nf3", "Nc6" };
            var record = new GameRecord("TestEvent", new DateTime(2024, 1, 1), "White", "Black", moves, false);
            Assert.That(record.Event, Is.EqualTo("TestEvent"));
            Assert.That(record.Date, Is.EqualTo(new DateTime(2024, 1, 1)));
            Assert.That(record.PlayingWhite, Is.EqualTo("White"));
            Assert.That(record.PlayingBlack, Is.EqualTo("Black"));
            Assert.That(record.MoveCount, Is.EqualTo(moves.Length));
            Assert.That(record.RoundCount, Is.EqualTo(2));
        }

        [Test]
        public void MovesAndRounds_ShouldReturnExpectedValues()
        {
            var moves = new[] { "e4", "e5", "Nf3", "Nc6" };
            var record = new GameRecord("Event", DateTime.Today, "W", "B", moves, false);
            Assert.That(record.Moves.Count(), Is.EqualTo(4));
            Assert.That(record.Rounds.Count(), Is.EqualTo(2));
        }
    }
}
