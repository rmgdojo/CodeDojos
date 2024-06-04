namespace CHIP_8_Virtual_Machine.InstructionBases;

public abstract class Instruction
{
    public string Mnemonic { get; init; }
    public Tribble Arguments { get; set; }

    public virtual void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public Instruction(Nibble high, Nibble middle, Nibble low)
        : this(new Tribble(high, middle, low))
    { }

    public Instruction(Nibble high, byte lowByte)
        : this(new Tribble(high, lowByte.HighNibble(), lowByte.LowNibble()))
    { }

    public Instruction(Tribble arguments)
    {
        Arguments = arguments;
        Mnemonic = GetType().Name.ToUpper();
    }
}
