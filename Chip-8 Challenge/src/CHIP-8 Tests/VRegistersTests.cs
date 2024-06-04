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
            Nibble index = 0;

            registers[index] = value;

            Assert.That(value, Is.EqualTo(registers[index]));
        }

        [Test]
        public void Register_Overflows_On_Increment()
        {
            VRegisters registers = new VRegisters();
            registers[0] = 0xFF;
            registers[0] += 1;

            Assert.That(registers[0], Is.EqualTo(0));
        }

        [Test]
        public void Register_Underflows_On_Decrement()
        {
            VRegisters registers = new VRegisters();
            registers[0] = 0x00;
            registers[0] -= 1;

            Assert.That(registers[0], Is.EqualTo(0xFF));
        }
    }
}
