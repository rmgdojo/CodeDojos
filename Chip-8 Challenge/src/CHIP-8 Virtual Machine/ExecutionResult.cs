using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine
{
    public class ExecutionResult
    {
        public Instruction Instruction { get; init; }
        public string Opcode { get; init; }
        public Tribble PC { get; init; }
        public Tribble I { get; init; }
        public bool F { get; init; }
        public VRegisters V { get; init; }
        public Stack<Tribble> Stack { get; init; }
        public bool[] Keystate { get; init; }

        public ExecutionResult(Instruction instruction, ushort opcode, Tribble PC, Tribble I, bool F, VRegisters V, Stack<Tribble> stack, bool[] keyState)
        {
            Instruction = instruction;
            Opcode = opcode.ToString("X4");
            this.PC = PC;
            this.I = I;
            this.F = F;
            this.V = new VRegisters(V);
            this.Stack = new Stack<Tribble>(stack);
            Keystate = keyState;
        }
    }
}