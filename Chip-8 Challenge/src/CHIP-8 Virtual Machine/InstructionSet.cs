namespace CHIP_8_Virtual_Machine;
using Instructions;
using System.Runtime.Intrinsics.X86;

public static class InstructionSet
{
    private static Dictionary<string, Func<TwelveBit, Instruction>> _instructions = new();

    public static Instruction GetByMnemonic(string mnemonic, TwelveBit arguments) =>
        _instructions[mnemonic](arguments);

    static InstructionSet()
    {
        _instructions.Add("CLR", (address) => new CLR(address));
        _instructions.Add("RTS", (address) => new RTS(address));
        _instructions.Add("SYS", (address) => new SYS(address));
        _instructions.Add("JUMP", (address) => new JUMP(address));
        _instructions.Add("CALL", (address) => new CALL(address));
        _instructions.Add("SKE", (address) => new SKE(address));
        _instructions.Add("SKNE", (address) => new SKNE(address));
        _instructions.Add("SKRE", (address) => new SKRE(address));
        _instructions.Add("LOAD", (address) => new LOAD(address));
        _instructions.Add("ADD", (address) => new ADD(address));
        _instructions.Add("MOVE", (address) => new MOVE(address));
        _instructions.Add("OR", (address) => new OR(address));
        _instructions.Add("AND", (address) => new AND(address));
        _instructions.Add("ADDR", (address) => new ADDR(address));
        _instructions.Add("XOR", (address) => new XOR(address));
        _instructions.Add("SUB", (address) => new SUB(address));
        _instructions.Add("SHR", (address) => new SHR(address));
        _instructions.Add("SUBR", (address) => new SUBR(address));
        _instructions.Add("SHL", (address) => new SHL(address));
        _instructions.Add("NOP", (address) => new NOP(address));
        _instructions.Add("SKRNE", (address) => new SKRNE(address));
        _instructions.Add("LOADI", (address) => new LOADI(address));
        _instructions.Add("JUMPI", (address) => new JUMPI(address));
        _instructions.Add("RAND", (address) => new RAND(address));
        _instructions.Add("DRAW", (address) => new DRAW(address));
        _instructions.Add("SKPR", (address) => new SKPR(address));
        _instructions.Add("SKUP", (address) => new SKUP(address));
        _instructions.Add("MOVED", (address) => new MOVED(address));
        _instructions.Add("KEYD", (address) => new KEYD(address));
        _instructions.Add("LOADD", (address) => new LOADD(address));
        _instructions.Add("LOADS", (address) => new LOADS(address));
        _instructions.Add("ADDI", (address) => new ADDI(address));
        _instructions.Add("LDSPR", (address) => new LDPSR(address));
        _instructions.Add("BCD", (address) => new BCD(address));
        _instructions.Add("STOR", (address) => new STOR(address));
        _instructions.Add("READ", (address) => new READ(address));
    }
}
