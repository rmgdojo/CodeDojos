using NUnit.Framework;
using RMGChess.Core;
using System;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class PGNConverterTests
    {
        [Test]
        public void GetGameRecordsFromPGN_ShouldParseSingleGame()
        {
            string pgn = "[Event \"TestEvent\"]\n[Date \"2024.01.01\"]\n[White \"White\"]\n[Black \"Black\"]\n\n1. e4 e5 2. Nf3 Nc6 1-0";
            var games = PGNConverter.GetGameRecordsFromPGN(pgn);
            Assert.That(games, Is.Not.Null);
            Assert.That(games.Count, Is.EqualTo(1));
            var game = games[0];
            Assert.That(game.Event, Is.EqualTo("TestEvent"));
            Assert.That(game.PlayingWhite, Is.EqualTo("White"));
            Assert.That(game.PlayingBlack, Is.EqualTo("Black"));
            Assert.That(game.MoveCount, Is.EqualTo(4));
        }

        [Test]
        public void GetGameRecordsFromPGN_InvalidFormat_ShouldThrow()
        {
            string pgn = "Invalid PGN";
            Assert.Throws<InvalidDataException>(() => PGNConverter.GetGameRecordsFromPGN(pgn));
        }
    }
}
