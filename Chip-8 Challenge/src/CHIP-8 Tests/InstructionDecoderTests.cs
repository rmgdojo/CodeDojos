using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    public class InstructionDecoderTests
    {
        [Test]
        public void Decode_WhenOpcodeIs0x00E0_ReturnsClear()
        {
            // Arrange
            ushort opcode = 0x00E0;

            // Act
            string result = InstructionDecoder.Decode(opcode);

            // Assert
            Assert.That(result, Is.EqualTo("CLEAR"));
        }

        [Test]
        public void Decode_WhenOpcodeIs0x00EE_ReturnsRet()
        {
            // Arrange
            ushort opcode = 0x00EE;

            // Act
            string result = InstructionDecoder.Decode(opcode);

            // Assert
            Assert.That(result, Is.EqualTo("RET"));
        }

        [Test]
        public void Decode_WhenOpcodeIs0x0NNN_ReturnsCall()
        {
            // Arrange
            ushort opcode = 0x0ABC;

            // Act
            string result = InstructionDecoder.Decode(opcode);

            // Assert
            Assert.That(result, Is.EqualTo("CALL"));
        }

        [Test]
        public void Decode_WhenOpcodeIsUnknown_ReturnsNope()
        {
            // Arrange
            ushort opcode = 0x1234;

            // Act
            string result = InstructionDecoder.Decode(opcode);

            // Assert
            Assert.That(result, Is.EqualTo("NOPE"));
        }
    }
}
