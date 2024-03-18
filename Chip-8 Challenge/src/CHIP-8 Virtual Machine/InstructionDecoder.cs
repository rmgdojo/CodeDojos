using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine;

public static class InstructionDecoder
{
    public static (string Mnemonic, TwelveBit Arguments) Decode(ushort opcode)
    {
        string opcodeString = opcode.ToString("X4");
        var arguments = GetArguments(opcode);

        switch (opcodeString[0])
        {
            case '0':
                {
                    return opcodeString[1..^0] switch
                    {
                        "0E0" => ("CLEAR", arguments),
                        "0EE" => ("RET", arguments),
                        _ => ("CALL", arguments)
                    };
                }
            case '1':
                {
                    return ("GOTO", arguments);
                }
            case '2':
                {
                    return ("CALL", arguments);
                }
            case '3':
                {
                    return ("SKIP_IF_EQUAL", arguments);
                }
            case '4':
                {
                    return ("SKIP_IF_NOT_EQUAL", arguments);
                }
            case '5':
                {
                    return ("SKIP_IF_REGISTER_EQUAL", arguments);
                }
            case '6':
                {
                    return ("SET_REGISTER", arguments);
                }
            case '7':
                {
                    return ("ADD_TO_REGISTER", arguments);
                }
            case '8':
                {
                    return opcodeString[3] switch
                    {
                        '0' => ("SET_REGISTER_TO_REGISTER", arguments),
                        '1' => ("OR_REGISTER_WITH_REGISTER", arguments),
                        '2' => ("AND_REGISTER_WITH_REGISTER", arguments),
                        '3' => ("XOR_REGISTER_WITH_REGISTER", arguments),
                        '4' => ("ADD_REGISTER_TO_REGISTER", arguments),
                        '5' => ("SUBTRACT_REGISTER_FROM_REGISTER", arguments),
                        '6' => ("SHIFT_RIGHT_REGISTER", arguments),
                        '7' => ("SUBTRACT_REGISTER_FROM_REGISTER_REVERSED", arguments),
                        'E' => ("SHIFT_LEFT_REGISTER", arguments),
                        _ => ("NOP", arguments)
                    };
                }
            case '9':
                {
                    return ("SKIP_IF_REGISTER_NOT_EQUAL", arguments);
                }
            case 'A':
                {
                    return ("SET_INDEX_REGISTER", arguments);
                }
            case 'B':
                {
                    return ("JUMP_WITH_OFFSET", arguments);
                }
            case 'C':
                {
                    return ("RANDOM", arguments);
                }
            case 'D':
                {
                    return ("DRAW", arguments);
                }
            case 'E':
                {
                    return opcodeString[2..^0] switch
                    {
                        "9E" => ("SKIP_IF_KEY_PRESSED", arguments),
                        "A1" => ("SKIP_IF_KEY_NOT_PRESSED", arguments),
                        _ => ("NOP", arguments)
                    };
                }
            case 'F':
                {
                    return opcodeString[2..^0] switch
                    {
                        "07" => ("GET_DELAY_TIMER", arguments),
                        "0A" => ("WAIT_FOR_KEY_PRESS", arguments),
                        "15" => ("SET_DELAY_TIMER", arguments),
                        "18" => ("SET_SOUND_TIMER", arguments),
                        "1E" => ("ADD_TO_INDEX_REGISTER", arguments),
                        "29" => ("SET_INDEX_REGISTER_TO_SPRITE", arguments),
                        "33" => ("STORE_BCD", arguments),
                        "55" => ("STORE_REGISTERS", arguments),
                        "65" => ("LOAD_REGISTERS", arguments),
                        _ => ("NOP", arguments)
                    };
                }
        }

        return ("NOP", arguments);
    }

    private static TwelveBit GetArguments(ushort opcode) =>
        new((Nybble)((opcode & 0x0F00) >> 8),
            (Nybble)((opcode & 0x00F0) >> 4),
            (Nybble)(opcode & 0x000F));
}