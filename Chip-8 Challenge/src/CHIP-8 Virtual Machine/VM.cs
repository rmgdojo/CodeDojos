using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine
{
    public class VM : IDisposable, IDebugVM
    {
        private RAM _ram;
        private VRegisters _vregisters;
        private Stack<Tribble> _stack;

        private bool _running;
        private Thread _instructionThread;

        private Keypad _keypad;
        private Display _display;
        private SystemFont _systemFont;
        private System.Timers.Timer _clockTick;
        private ITimer _delayTimer;
        private ITimer _soundTimer;

        public RAM RAM => _ram;
        public Keypad Keypad => _keypad;
        public Display Display => _display;
        public System.Timers.Timer ClockTick => _clockTick;
        public ITimer DelayTimer => _delayTimer;
        public ITimer SoundTimer => _soundTimer;
        public SystemFont SystemFont => _systemFont;

        public VRegisters V => _vregisters;

        public Tribble PC { get; set; }
        public Tribble I { get; set; }
        public byte F { get { return V[0xF]; } }

        public event EventHandler<ExecutionResult> OnAfterExecution;

        public VM(IKeypadMap keypadMap = null)
        {
            _ram = new RAM();
            _vregisters = new VRegisters();
            _stack = new Stack<Tribble>();
            _keypad = new Keypad(keypadMap ?? new WindowsKeypadMap());
            _display = new Display(this);
            _delayTimer = new Timer();
            _soundTimer = new Timer();
            _clockTick = new System.Timers.Timer();
            _clockTick.Stop();
            // load system font into memory
            _systemFont = new SystemFont();
            _systemFont.InstallTo(_ram);
        }

        void IDebugVM.ReplaceTimers(CHIP_8_Virtual_Machine.ITimer delayTimer, CHIP_8_Virtual_Machine.ITimer soundTimer)
        {
            _delayTimer = delayTimer;
            _soundTimer = soundTimer;
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

        internal void SetFlag(bool value)
        {
            V[0xF] = (byte)(value ? 1 : 0);
        }

        internal void PushStack(Tribble value)
        {
            _stack.Push(value);
        }

        internal Tribble PopStack()
        {
            return _stack.Pop();
        }

        private void InstructionCycles()
        {
            while (_running)
            {
                InstructionCycle();
            }
        }

        private void InstructionCycle()
        {
            ushort opcode = _ram.GetWord(PC);
            Instruction instruction = InstructionDecoder.DecodeInstruction(opcode);
            ushort pc = PC;
            PC += (PC + 2 < 0xFFF) ? 2 : 0;

            instruction.Execute(this);
            ExecutionResult result = new ExecutionResult(instruction, opcode, pc, I, F, V, _stack, _keypad.State);
            OnAfterExecution?.Invoke(this, result);

            if (PC == 0xFFF)
            {
                Console.WriteLine("End of memory reached");
                _running = false; 
                return;
            }
        }

        public void Run(bool threading = true)
        {
            _running=true;   
            if (threading)
            {
                _instructionThread = new Thread(InstructionCycles);
                _instructionThread.IsBackground = true;
                 _instructionThread.Start();
            }
            else
            {
                ClockTick.Interval = 2;
                ClockTick.Elapsed += (s,e) => Task.Run(InstructionCycle);
                ClockTick.Elapsed += (s, e) => { if (_running) ClockTick.Start(); };
                ClockTick.Start();
            }
            
        }

        public void Stop()
        {
            _running = false; 
        }

        public void Dispose()
        {
            _running = false;
        }
    }
}
