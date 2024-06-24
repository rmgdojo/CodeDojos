using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine;

public static class InstructionDecoder
{
    public static Instruction DecodeInstruction(ushort instructionData)
    {
        var decoded = Decode(instructionData);
        return InstructionSet.GetByMnemonic(decoded.Mnemonic, decoded.Arguments);
    }

    private static (string Mnemonic, Tribble Arguments) Decode(ushort opcode)
    {
        Tribble arguments = (ushort)(opcode & 0x0FFF);
        Nibble opcodePrefix = (byte)(opcode >> 12);

        switch (opcodePrefix)
        {
            case 0x0:
                {
                    return opcode switch
                    {
                        0x00E0 => ("CLR", arguments),
                        0x00EE => ("RTS", arguments),
                        _ => ("SYS", arguments)
                    };
                }
            case 0x1:
                {
                    return ("JUMP", arguments);
                }
            case 0x2:
                {
                    return ("CALL", arguments);
                }
            case 0x3:
                {
                    return ("SKE", arguments);
                }
            case 0x4:
                {
                    return ("SKNE", arguments);
                }
            case 0x5:
                {
                    return ("SKRE", arguments);
                }
            case 0x6:
                {
                    return ("LOAD", arguments);
                }
            case 0x7:
                {
                    return ("ADD", arguments);
                }
            case 0x8:
                {
                    return (byte)arguments.LowNibble switch
                    {
                        0x0 => ("MOVE", arguments),
                        0x1 => ("OR", arguments),
                        0x2 => ("AND", arguments),
                        0x3 => ("XOR", arguments),
                        0x4 => ("ADDR", arguments),
                        0x5 => ("SUB", arguments),
                        0x6 => ("SHR", arguments),
                        0x7 => ("SUBR", arguments),
                        0xE => ("SHL", arguments),
                        _ => ("NOP", arguments)
                    };
                }
            case 0x9:
                {
                    return ("SKRNE", arguments);
                }
            case 0xA:
                {
                    return ("LOADI", arguments);
                }
            case 0xB:
                {
                    return ("JUMPI", arguments);
                }
            case 0xC:
                {
                    return ("RAND", arguments);
                }
            case 0xD:
                {
                    return ("DRAW", arguments);
                }
            case 0xE:
                {
                    return arguments.LowByte switch
                    {
                        0x9E => ("SKPR", arguments),
                        0xA1 => ("SKUP", arguments),
                        _ => ("NOP", arguments)
                    };
                }
            case 0xF:
                {
                    return arguments.LowByte switch
                    {
                        0x07 => ("MOVED", arguments),
                        0x0A => ("KEYD", arguments),
                        0x15 => ("LOADD", arguments),
                        0x18 => ("LOADS", arguments),
                        0x1E => ("ADDI", arguments),
                        0x29 => ("LDSPR", arguments),
                        0x33 => ("BCD", arguments),
                        0x55 => ("STOR", arguments),
                        0x65 => ("READ", arguments),
                        _ => ("NOP", arguments)
                    };
                }
        }

        return ("NOP", arguments);
    }
}
