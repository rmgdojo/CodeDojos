using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Tests
{
    public class InstructionDecoderTests
    {
        [TestCase((ushort)0x00E0, ExpectedResult = "CLEAR")]
        [TestCase((ushort)0x00EE, ExpectedResult = "RET")]
        [TestCase((ushort)0x0ABC, ExpectedResult = "CALL")]
        [TestCase((ushort)0xFFFF, ExpectedResult = "NOP")]
        [TestCase((ushort)0x00E0, ExpectedResult = "CLEAR")]
        [TestCase((ushort)0x00EE, ExpectedResult = "RET")]
        [TestCase((ushort)0x1000, ExpectedResult = "GOTO")]
        [TestCase((ushort)0x2000, ExpectedResult = "CALL")]
        [TestCase((ushort)0x3000, ExpectedResult = "SKIP_IF_EQUAL")]
        [TestCase((ushort)0x4000, ExpectedResult = "SKIP_IF_NOT_EQUAL")]
        [TestCase((ushort)0x5000, ExpectedResult = "SKIP_IF_REGISTER_EQUAL")]
        [TestCase((ushort)0x6000, ExpectedResult = "SET_REGISTER")]
        [TestCase((ushort)0x7000, ExpectedResult = "ADD_TO_REGISTER")]
        [TestCase((ushort)0x8000, ExpectedResult = "SET_REGISTER_TO_REGISTER")]
        [TestCase((ushort)0x8001, ExpectedResult = "OR_REGISTER_WITH_REGISTER")]
        [TestCase((ushort)0x8002, ExpectedResult = "AND_REGISTER_WITH_REGISTER")]
        [TestCase((ushort)0x8003, ExpectedResult = "XOR_REGISTER_WITH_REGISTER")]
        [TestCase((ushort)0x8004, ExpectedResult = "ADD_REGISTER_TO_REGISTER")]
        [TestCase((ushort)0x8005, ExpectedResult = "SUBTRACT_REGISTER_FROM_REGISTER")]
        [TestCase((ushort)0x8006, ExpectedResult = "SHIFT_RIGHT_REGISTER")]
        [TestCase((ushort)0x8007, ExpectedResult = "SUBTRACT_REGISTER_FROM_REGISTER_REVERSED")]
        [TestCase((ushort)0x800E, ExpectedResult = "SHIFT_LEFT_REGISTER")]
        [TestCase((ushort)0x9000, ExpectedResult = "SKIP_IF_REGISTER_NOT_EQUAL")]
        [TestCase((ushort)0xA000, ExpectedResult = "SET_INDEX_REGISTER")]
        [TestCase((ushort)0xB000, ExpectedResult = "JUMP_WITH_OFFSET")]
        [TestCase((ushort)0xC000, ExpectedResult = "RANDOM")]
        [TestCase((ushort)0xD000, ExpectedResult = "DRAW")]
        [TestCase((ushort)0xE09E, ExpectedResult = "SKIP_IF_KEY_PRESSED")]
        [TestCase((ushort)0xE0A1, ExpectedResult = "SKIP_IF_KEY_NOT_PRESSED")]
        [TestCase((ushort)0xF007, ExpectedResult = "GET_DELAY_TIMER")]
        [TestCase((ushort)0xF00A, ExpectedResult = "WAIT_FOR_KEY_PRESS")]
        [TestCase((ushort)0xF015, ExpectedResult = "SET_DELAY_TIMER")]
        [TestCase((ushort)0xF018, ExpectedResult = "SET_SOUND_TIMER")]
        [TestCase((ushort)0xF01E, ExpectedResult = "ADD_TO_INDEX_REGISTER")]
        [TestCase((ushort)0xF029, ExpectedResult = "SET_INDEX_REGISTER_TO_SPRITE")]
        [TestCase((ushort)0xF033, ExpectedResult = "STORE_BCD")]
        [TestCase((ushort)0xF055, ExpectedResult = "STORE_REGISTERS")]
        [TestCase((ushort)0xF065, ExpectedResult = "LOAD_REGISTERS")]
        [TestCase((ushort)0xFFFF, ExpectedResult = "NOP")]
        public string Decode_WhenOpcodeIs0x00E0_ReturnsClear(ushort opcode)
        {   
            var result = InstructionDecoder.Decode(opcode);
            return result.Mnemonic;
        }
    }
}
