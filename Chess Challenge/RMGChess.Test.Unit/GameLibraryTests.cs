using NUnit.Framework;
using RMGChess.Core;
using System.Collections.Generic;
using System.Linq;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class GameLibraryTests
    {
        [Test]
        public void MagnusCarlsenGames_ShouldReturnReadOnlyList()
        {
            // The resource may not exist in test context, so just check type and non-null
            var games = GameLibrary.MagnusCarlsenGames;
            Assert.That(games, Is.Not.Null);
            Assert.That(games, Is.InstanceOf<IReadOnlyList<GameRecord>>());
        }

        [Test]
        public void MagnusCarlsenGames_ShouldBeCached()
        {
            var games1 = GameLibrary.MagnusCarlsenGames;
            var games2 = GameLibrary.MagnusCarlsenGames;
            Assert.That(ReferenceEquals(games1, games2), Is.True);
        }
    }
}
