using NUnit.Framework;
using RMGChess.Core;

namespace RMGChess.Test.Unit
{
    [TestFixture]
    public class PlayControlTests
    {
        [Test]
        public void Constructor_DefaultValues_ShouldSetProperties()
        {
            var control = new PlayControl();
            Assert.That(control.Stop, Is.False);
            Assert.That(control.GoToRound, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_CustomValues_ShouldSetProperties()
        {
            var control = new PlayControl(true, 5.5f);
            Assert.That(control.Stop, Is.True);
            Assert.That(control.GoToRound, Is.EqualTo(5.5f));
        }

        [Test]
        public void Properties_CanBeSet()
        {
            var control = new PlayControl();
            control.Stop = true;
            control.GoToRound = 3.2f;
            Assert.That(control.Stop, Is.True);
            Assert.That(control.GoToRound, Is.EqualTo(3.2f));
        }
    }
}
