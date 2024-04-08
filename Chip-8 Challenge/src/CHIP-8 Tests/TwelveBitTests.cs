using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    public class TwelveBitTests
    {
        [Test]
        public void LowByte_GetValue_ReturnsCorrectValue()
        {
            var twelveBit = new TwelveBit(0xAB3);

            var lowByte = twelveBit.LowByte;

            Assert.That(0xB3, Is.EqualTo(lowByte));
        }

        [Test]
        public void HighByte_GetValue_ReturnsCorrectValue()
        {
            var twelveBit = new TwelveBit(0xAB3);

            var highByte = twelveBit.HighByte;

            Assert.That(0xAB, Is.EqualTo(highByte));
        }

        [Test]
        public void ToString_ReturnsCorrectStringRepresentation()
        {
            var twelveBit = new TwelveBit(0xAB3);

            var result = twelveBit.ToString();

            Assert.That("AB3", Is.EqualTo(result));
        }

        [Test]
        public void ImplicitConversion_FromTwelveBitToUShort_ReturnsCorrectValue()
        {
            TwelveBit twelveBit = 0xAB3;

            ushort result = twelveBit;

            Assert.That(result == twelveBit);
        }

        [Test]
        public void ImplicitConversion_FromUShortToTwelveBit_ReturnsCorrectValue()
        {
            ushort value = 0xAB3;

            TwelveBit result = value;

            Assert.That(result == value);
        }
    }
}
