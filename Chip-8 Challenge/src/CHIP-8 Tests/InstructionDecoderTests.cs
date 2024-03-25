using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    public class InstructionDecoderTests
    {
        [TestCase((ushort)0x00E0, ExpectedResult = "CLR")]
        [TestCase((ushort)0x00EE, ExpectedResult = "RTS")]
        [TestCase((ushort)0x0ABC, ExpectedResult = "SYS")]
        [TestCase((ushort)0x1000, ExpectedResult = "JUMP")]
        [TestCase((ushort)0x2000, ExpectedResult = "CALL")]
        [TestCase((ushort)0x3000, ExpectedResult = "SKE")]
        [TestCase((ushort)0x4000, ExpectedResult = "SKNE")]
        [TestCase((ushort)0x5000, ExpectedResult = "SKRE")]
        [TestCase((ushort)0x6000, ExpectedResult = "LOAD")]
        [TestCase((ushort)0x7000, ExpectedResult = "ADD")]
        [TestCase((ushort)0x8000, ExpectedResult = "MOVE")]
        [TestCase((ushort)0x8001, ExpectedResult = "OR")]
        [TestCase((ushort)0x8002, ExpectedResult = "AND")]
        [TestCase((ushort)0x8003, ExpectedResult = "XOR")]
        [TestCase((ushort)0x8004, ExpectedResult = "ADDR")]
        [TestCase((ushort)0x8005, ExpectedResult = "SUB")]
        [TestCase((ushort)0x8006, ExpectedResult = "SHR")]
        [TestCase((ushort)0x8007, ExpectedResult = "SUBR")]
        [TestCase((ushort)0x800E, ExpectedResult = "SHL")]
        [TestCase((ushort)0x9000, ExpectedResult = "SKRNE")]
        [TestCase((ushort)0xA000, ExpectedResult = "LOADI")]
        [TestCase((ushort)0xB000, ExpectedResult = "JUMPI")]
        [TestCase((ushort)0xC000, ExpectedResult = "RAND")]
        [TestCase((ushort)0xD000, ExpectedResult = "DRAW")]
        [TestCase((ushort)0xE09E, ExpectedResult = "SKPR")]
        [TestCase((ushort)0xE0A1, ExpectedResult = "SKUP")]
        [TestCase((ushort)0xF007, ExpectedResult = "MOVED")]
        [TestCase((ushort)0xF00A, ExpectedResult = "KEYD")]
        [TestCase((ushort)0xF015, ExpectedResult = "LOADD")]
        [TestCase((ushort)0xF018, ExpectedResult = "LOADS")]
        [TestCase((ushort)0xF01E, ExpectedResult = "ADDI")]
        [TestCase((ushort)0xF029, ExpectedResult = "LDSPR")]
        [TestCase((ushort)0xF033, ExpectedResult = "BCD")]
        [TestCase((ushort)0xF055, ExpectedResult = "STOR")]
        [TestCase((ushort)0xF065, ExpectedResult = "READ")]
        [TestCase((ushort)0xFFFF, ExpectedResult = "NOP")]
        public string Decode_WhenOpcodeIs0x00E0_ReturnsClear(ushort opcode)
        {   
            var result = InstructionDecoder.Decode(opcode);
            return result.Mnemonic;
        }
    }
}
