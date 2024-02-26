using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    public class VRegistersTests
    {
        [Test]
        public void Get_Set_RegisterValue()
        {
            VRegisters registers = new VRegisters();

            byte value = 0x42;
            int index = 0;

            registers[index] = value;

            Assert.That(value, Is.EqualTo(registers[index]));
        }

        [Test]
        public void Get_Set_InvalidIndex_ThrowsException()
        {
            VRegisters registers = new VRegisters();

            byte value = 0x42;
            int invalidIndex = VRegisters.MAX_REGISTERS;

            Assert.Throws<ArgumentOutOfRangeException>(() => registers[invalidIndex] = value);
        }
    }
}
