using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine
{
    public static class InstructionDecoder
    {
        public static string Decode(ushort opcode)
        {
            string opcodeString = opcode.ToString("X4");
            switch(opcodeString[0])
            {
                case '0':
                    Func<string> microcode = opcodeString[1..^0] switch
                    {
                        "0E0" => () => { return "CLEAR"; },
                        "0EE" => () => { return "RET"; },
                        _ => () => { return "CALL"; }
                    };
                    return microcode();
            }

            return "NOPE";
        }

        private static Nybble[] GetOpcodeNybbles(ushort opcode)
        {
            Nybble[] nybbles = new Nybble[4];
            nybbles[0] = (Nybble)((opcode & 0xF000) >> 12);
            nybbles[1] = (Nybble)((opcode & 0x0F00) >> 8);
            nybbles[2] = (Nybble)((opcode & 0x00F0) >> 4);
            nybbles[3] = (Nybble)(opcode & 0x000F);
            return nybbles;
        }
    }
}
