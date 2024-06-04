using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    public class TribbleTests
    {
        [Test]
        public void LowByte_GetValue_ReturnsCorrectValue()
        {
            var Tribble = new Tribble(0xAB3);
            var lowByte = Tribble.LowByte;

            Assert.That(0xB3, Is.EqualTo(lowByte));
        }

        [Test]
        public void HighByte_GetValue_ReturnsCorrectValue()
        {
            var Tribble = new Tribble(0xAB3);
            var highByte = Tribble.HighByte;

            Assert.That(0xAB, Is.EqualTo(highByte));
        }

        [Test]
        public void ToString_ReturnsCorrectStringRepresentation()
        {
            var Tribble = new Tribble(0xAB3);
            var result = Tribble.ToHexString();

            Assert.That("AB3", Is.EqualTo(result));
        }

        [Test]
        public void ImplicitConversion_FromTribbleToUShort_ReturnsCorrectValue()
        {
            Tribble Tribble = 0xAB3;
            ushort result = Tribble;

            Assert.That(result == Tribble);
        }

        [Test]
        public void ImplicitConversion_FromUShortToTribble_ReturnsCorrectValue()
        {
            ushort value = 0xAB3;
            Tribble result = value;

            Assert.That(result == value);
        }
    }
}
