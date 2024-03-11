using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine;

public static class InstructionDecoder
{
    public static (string Code, TwelveBit TwelveBitOpCode) Decode(ushort opcode)
    {
        string opcodeString = opcode.ToString("X4");

        var twelveBitOpcode = GetTwelveBitOpcode(opcode);

        switch (opcodeString[0])
        {
            case '0':
            {
                return opcodeString[1..^0] switch
                {
                    "0E0" => ("CLEAR", twelveBitOpcode),
                    "0EE" => ("RET", twelveBitOpcode),
                    _ => ("CALL", twelveBitOpcode)
                };
            }
            case '1':
            {
                return ("GOTO", twelveBitOpcode);
            }
            case '2':
            {
                return ("CALL", twelveBitOpcode);
            }
        }

        return ("NOP", twelveBitOpcode);
    }

    private static TwelveBit GetTwelveBitOpcode(ushort opcode) =>
        new((Nybble)((opcode & 0x0F00) >> 8),
            (Nybble)((opcode & 0x00F0) >> 4),
            (Nybble)(opcode & 0x000F));
}