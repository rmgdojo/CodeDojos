using CHIP_8_Virtual_Machine;

namespace CHIP_8_Console
{
    public class Program
    {
        private static VM _vm;
        static void Main(string[] args)
        {
            Console.WriteLine("Will load PONG and run. Press CTRL+C at any time to quit.");
            _vm = new VM(new WindowsKeypadMap());
            ((IDebugVM)_vm).ReplaceTimers(new DebugTimer(_vm), new DebugTimer(_vm));
            _vm.Load("pong.rom");
            _vm.OnAfterExecution += OnAfterExecution;
            _vm.Run(ClockMode.Threaded);
            while (true) ;
        }

        private static void OnAfterExecution(object sender, ExecutionResult e)
        {
            ((IDebugVM)_vm).Pause();

            Console.WriteLine($"PC:{e.PC.ToString("X3")}\t{GetOpcodeAndMnemonic()}\tI:{e.I.ToHexString()}\tF:{e.F.ToString("X2")}");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    char c = Console.ReadKey(true).KeyChar;
                    if (c == '\r')
                    {
                        ((IDebugVM)_vm).Resume(); 
                        break;
                    }
                }
            }

            string GetOpcodeAndMnemonic()
            {
                return $"{e.Opcode}\t{e.Instruction.Disassemble(_vm)}".PadRight(30); // need to ensure the mnemonic view is tabular
            }
        }
    }
}
