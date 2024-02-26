using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    [TestFixture]
    public class RAMTests
    {
        private RAM _ram;

        [SetUp]
        public void Setup()
        {
            _ram = new RAM();
        }

        [Test]
        public void Get_Set_ValidAddress_ReturnsCorrectValue()
        {
            ushort address = 0x200;
            byte value = 0xAB;

            _ram[address] = value;

            byte retrievedValue = _ram[address];

            Assert.That(value, Is.EqualTo(retrievedValue));
        }

        [Test]
        public void Get_Set_InvalidAddress_ThrowsException()
        {
            ushort address = 0x5000;
            byte value = 0xCD;

            Assert.Throws<ArgumentOutOfRangeException>(() => _ram[address] = value);
        }
    }
}
