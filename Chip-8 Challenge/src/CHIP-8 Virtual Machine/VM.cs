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
        private Stack<Tribble> _stack;

        public RAM RAM => _ram;
        public Tribble PC { get; set; }
        public Tribble I { get; set; }

        public VRegisters V => _vregisters;


        public VM()
        {
            _ram = new RAM();
            _vregisters = new VRegisters();
            _stack = new Stack<Tribble>();
        }

        public void Load(byte[] bytes)
        {
            if (bytes.Length > RAM.RAM_SIZE)
            {
                throw new ArgumentOutOfRangeException("ROM is too large to fit in RAM");
            }

            for (ushort i = 0; i < bytes.Length; i++)
            {
                PC = 0x200;
                _ram[(ushort)(i + PC)] = bytes[i];
            }
        }

        public void Load(string path)
        {
            byte[] rom = System.IO.File.ReadAllBytes(path);
            if (rom.Length > RAM.RAM_SIZE)
            {
                throw new ArgumentOutOfRangeException("ROM is too large to fit in RAM");
            }

            for (ushort i = 0; i < rom.Length; i++)
            {
                PC = 0x200;
                _ram[(ushort)(i + PC)] = rom[i];
            }
        }

        public void PushStack(Tribble value)
        {
            _stack.Push(value);
        }

        public Tribble PopStack()
        {
            return _stack.Pop();
        }

        public void Run()
        {
            while (true)
            {
                Instruction instruction = InstructionDecoder.DecodeInstruction(_ram.GetWord(PC));
                instruction.Execute(this);

                // if at the end of memory, stop    
                if (PC == 0xFFF) return;

                PC += 2;
            }
        }
    }
}