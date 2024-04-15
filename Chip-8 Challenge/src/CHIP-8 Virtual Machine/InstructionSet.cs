namespace CHIP_8_Virtual_Machine;
using Instructions;

public static class InstructionSet
{
    private static Dictionary<string, Func<TwelveBit, Instruction>> _instructions = new();

    public static Instruction GetByMnemonic(string mnemonic, TwelveBit arguments) =>
        _instructions[mnemonic](arguments);

    static InstructionSet()
    {
        _instructions.Add("SYS", (address) => new SYS(address));
    }
}
