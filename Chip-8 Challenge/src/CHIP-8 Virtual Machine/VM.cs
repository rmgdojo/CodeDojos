using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine
{
    public class VM
    {
        private RAM _ram;
        private VRegisters _vregisters;

        public VM()
        {
            _ram = new RAM();
            _vregisters = new VRegisters();
        }

        public void Load(string path)
        {
            byte[] rom = System.IO.File.ReadAllBytes(path);
            for (int i = 0; i < rom.Length; i++)
            {
                PC = 0x200;
                _ram[i + PC] = rom[i];
            }
        }

        public void Run()
        {
            while (true)
            {
                Instruction instruction = InstructionDecoder.Decode(_ram.GetWord(PC));
                Console.Write($"{instruction.Mnemonic} {instruction.Arguments}");
                Console.ReadLine();
                PC += 2;
            }
        }

        public TwelveBit PC { get; private set; }
        public TwelveBit I { get; private set; }

        public VRegisters V => _vregisters;
    }
}