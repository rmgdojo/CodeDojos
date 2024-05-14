namespace CHIP_8_Virtual_Machine;
using Instructions;
using System.Runtime.Intrinsics.X86;

public static class InstructionSet
{
    private static Dictionary<string, Func<Tribble, Instruction>> _instructions = new();

    public static Instruction GetByMnemonic(string mnemonic, Tribble arguments) =>
        _instructions[mnemonic](arguments);

    static InstructionSet()
    {
        _instructions.Add("CLR", (arguments) => new CLR(arguments));
        _instructions.Add("RTS", (arguments) => new RTS(arguments));
        _instructions.Add("SYS", (arguments) => new SYS(arguments));
        _instructions.Add("JUMP", (arguments) => new JUMP(arguments));
        _instructions.Add("CALL", (arguments) => new CALL(arguments));
        _instructions.Add("SKE", (arguments) => new SKE(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("SKNE", (arguments) => new SKNE(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("SKRE", (arguments) => new SKRE(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("LOAD", (arguments) => new LOAD(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("ADD", (arguments) => new ADD(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("MOVE", (arguments) => new MOVE(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("OR", (arguments) => new OR(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("AND", (arguments) => new AND(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("ADDR", (arguments) => new ADDR(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("XOR", (arguments) => new XOR(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("SUB", (arguments) => new SUB(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("SHR", (arguments) => new SHR(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("SUBR", (arguments) => new SUBR(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("SHL", (arguments) => new SHL(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("NOP", (arguments) => new NOP(arguments));
        _instructions.Add("SKRNE", (arguments) => new SKRNE(arguments.HighNybble, arguments.MiddleNybble));
        _instructions.Add("LOADI", (arguments) => new LOADI(arguments));
        _instructions.Add("JUMPI", (arguments) => new JUMPI(arguments));
        _instructions.Add("RAND", (arguments) => new RAND(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("DRAW", (arguments) => new DRAW(arguments.HighNybble, arguments.MiddleNybble, arguments.LowNybble));
        _instructions.Add("SKPR", (arguments) => new SKPR(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("SKUP", (arguments) => new SKUP(arguments.HighNybble, arguments.LowByte));
        _instructions.Add("MOVED", (arguments) => new MOVED(arguments.HighNybble));
        _instructions.Add("KEYD", (arguments) => new KEYD(arguments.HighNybble));
        _instructions.Add("LOADD", (arguments) => new LOADD(arguments.HighNybble));
        _instructions.Add("LOADS", (arguments) => new LOADS(arguments.HighNybble));
        _instructions.Add("ADDI", (arguments) => new ADDI(arguments.HighNybble));
        _instructions.Add("LDSPR", (arguments) => new LDPSR(arguments.HighNybble));
        _instructions.Add("BCD", (arguments) => new BCD(arguments.HighNybble));
        _instructions.Add("STOR", (arguments) => new STOR(arguments.HighNybble));
        _instructions.Add("READ", (arguments) => new READ(arguments.HighNybble));
    }
}
