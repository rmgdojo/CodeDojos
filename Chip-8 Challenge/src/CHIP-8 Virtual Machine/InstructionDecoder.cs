using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine;

public static class InstructionDecoder
{
    public static Instruction DecodeInstruction(Tribble instructionData)
    {
        var decoded = Decode(instructionData);
        return InstructionSet.GetByMnemonic(decoded.Mnemonic, decoded.Arguments);
    }

    private static (string Mnemonic, Tribble Arguments) Decode(ushort opcode)
    {
        string opcodeString = opcode.ToString("X4");
        var arguments = GetArguments(opcode);

        switch (opcodeString[0])
        {
            case '0':
                {
                    return opcodeString[1..^0] switch
                    {
                        "0E0" => ("CLR", arguments),
                        "0EE" => ("RTS", arguments),
                        _ => ("SYS", arguments)
                    };
                }
            case '1':
                {
                    return ("JUMP", arguments);
                }
            case '2':
                {
                    return ("CALL", arguments);
                }
            case '3':
                {
                    return ("SKE", arguments);
                }
            case '4':
                {
                    return ("SKNE", arguments);
                }
            case '5':
                {
                    return ("SKRE", arguments);
                }
            case '6':
                {
                    return ("LOAD", arguments);
                }
            case '7':
                {
                    return ("ADD", arguments);
                }
            case '8':
                {
                    return opcodeString[3] switch
                    {
                        '0' => ("MOVE", arguments),
                        '1' => ("OR", arguments),
                        '2' => ("AND", arguments),
                        '3' => ("XOR", arguments),
                        '4' => ("ADDR", arguments),
                        '5' => ("SUB", arguments),
                        '6' => ("SHR", arguments),
                        '7' => ("SUBR", arguments),
                        'E' => ("SHL", arguments),
                        _ => ("NOP", arguments)
                    };
                }
            case '9':
                {
                    return ("SKRNE", arguments);
                }
            case 'A':
                {
                    return ("LOADI", arguments);
                }
            case 'B':
                {
                    return ("JUMPI", arguments);
                }
            case 'C':
                {
                    return ("RAND", arguments);
                }
            case 'D':
                {
                    return ("DRAW", arguments);
                }
            case 'E':
                {
                    return opcodeString[2..^0] switch
                    {
                        "9E" => ("SKPR", arguments),
                        "A1" => ("SKUP", arguments),
                        _ => ("NOP", arguments)
                    };
                }
            case 'F':
                {
                    return opcodeString[2..^0] switch
                    {
                        "07" => ("MOVED", arguments),
                        "0A" => ("KEYD", arguments),
                        "15" => ("LOADD", arguments),
                        "18" => ("LOADS", arguments),
                        "1E" => ("ADDI", arguments),
                        "29" => ("LDSPR", arguments),
                        "33" => ("BCD", arguments),
                        "55" => ("STOR", arguments),
                        "65" => ("READ", arguments),
                        _ => ("NOP", arguments)
                    };
                }
        }

        return ("NOP", arguments);
    }

    private static Tribble GetArguments(ushort opcode) =>
        new((Nibble)((opcode & 0x0F00) >> 8),
            (Nibble)((opcode & 0x00F0) >> 4),
            (Nibble)(opcode & 0x000F));
}
