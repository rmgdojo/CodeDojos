namespace CHIP_8_Virtual_Machine;

using CHIP_8_Virtual_Machine.InstructionBases;
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
        _instructions.Add("SKE", (arguments) => new SKE(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("SKNE", (arguments) => new SKNE(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("SKRE", (arguments) => new SKRE(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("LOAD", (arguments) => new LOAD(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("ADD", (arguments) => new ADD(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("MOVE", (arguments) => new MOVE(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("OR", (arguments) => new OR(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("AND", (arguments) => new AND(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("ADDR", (arguments) => new ADDR(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("XOR", (arguments) => new XOR(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("SUB", (arguments) => new SUB(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("SHR", (arguments) => new SHR(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("SUBR", (arguments) => new SUBR(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("SHL", (arguments) => new SHL(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("NOP", (arguments) => new NOP(arguments));
        _instructions.Add("SKRNE", (arguments) => new SKRNE(arguments.HighNibble, arguments.MiddleNibble));
        _instructions.Add("LOADI", (arguments) => new LOADI(arguments));
        _instructions.Add("JUMPI", (arguments) => new JUMPI(arguments));
        _instructions.Add("RAND", (arguments) => new RAND(arguments.HighNibble, arguments.LowByte));
        _instructions.Add("DRAW", (arguments) => new DRAW(arguments.HighNibble, arguments.MiddleNibble, arguments.LowNibble));
        _instructions.Add("SKPR", (arguments) => new SKPR(arguments.HighNibble));
        _instructions.Add("SKUP", (arguments) => new SKUP(arguments.HighNibble));
        _instructions.Add("MOVED", (arguments) => new MOVED(arguments.HighNibble));
        _instructions.Add("KEYD", (arguments) => new KEYD(arguments.HighNibble));
        _instructions.Add("LOADD", (arguments) => new LOADD(arguments.HighNibble));
        _instructions.Add("LOADS", (arguments) => new LOADS(arguments.HighNibble));
        _instructions.Add("ADDI", (arguments) => new ADDI(arguments.HighNibble));
        _instructions.Add("LDSPR", (arguments) => new LDSPR(arguments.HighNibble));
        _instructions.Add("BCD", (arguments) => new BCD(arguments.HighNibble));
        _instructions.Add("STOR", (arguments) => new STOR(arguments.HighNibble));
        _instructions.Add("READ", (arguments) => new READ(arguments.HighNibble));
    }
}
