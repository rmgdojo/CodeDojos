using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine
{
    public class VM : IDisposable
    {
        private RAM _ram;
        private VRegisters _vregisters;
        private Stack<Tribble> _stack;

        private bool _running;
        private Thread _instructionThread;

        private Keypad _keypad;
        private Display _display;
        private Timer _delayTimer;
        private Timer _soundTimer;

        public RAM RAM => _ram;
        public Tribble PC { get; set; }
        public Tribble I { get; set; }
        public byte F { get { return V[0xF]; } set { V[0xF] = value; } }
        public Keypad Keypad => _keypad;
        public Display Display => _display;
        public Timer DelayTimer => _delayTimer;
        public Timer SoundTimer => _soundTimer;

        public VRegisters V => _vregisters;

        public VM()
        {
            _ram = new RAM();
            _vregisters = new VRegisters();
            _stack = new Stack<Tribble>();
            _keypad = new Keypad();
            _display = new Display();
            _delayTimer = new Timer();
            _soundTimer = new Timer();

            // load system font into memory
            _ram.SetBytes(SystemFont.Address, SystemFont.Bytes);
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
                _ram[i + PC] = bytes[i];
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
                _ram[i + PC] = rom[i];
            }
        }

        internal void PushStack(Tribble value)
        {
            _stack.Push(value);
        }

        internal Tribble PopStack()
        {
            return _stack.Pop();
        }

        private void InstructionCycle()
        {
            while (_running)
            {
                Instruction instruction = InstructionDecoder.DecodeInstruction(_ram.GetWord(PC));
                ushort pcUshort = PC;
                pcUshort += 2;
                // if at the end of memory, stop    
                if (pcUshort < 0xFFF)
                {
                    PC += 2;
                }

                instruction.Execute(this);
                if (PC == 0xFFF) return;
            }
        }

        public void Run()
        {
            _instructionThread = new Thread(InstructionCycle);
            _running = true;
            _instructionThread.Start();
        }

        public void Dispose()
        {
            _running = false;
        }
    }
}