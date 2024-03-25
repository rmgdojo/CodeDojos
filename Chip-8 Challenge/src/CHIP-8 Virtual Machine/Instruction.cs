namespace CHIP_8_Virtual_Machine;

public class Instruction : IInstruction
{
    public string Mnemonic { get; set; }
    public TwelveBit Arguments { get; set; }

    public Instruction(string mnemonic, TwelveBit arguments)
    {
        Mnemonic = mnemonic;
        Arguments = arguments;
    }

    // implicit conversion from (string, TwelveBit) to Instruction
    public static implicit operator Instruction((string mnemonic, TwelveBit arguments) tuple) =>
        new Instruction(tuple.mnemonic, tuple.arguments);
}
