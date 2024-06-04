namespace CHIP_8_Virtual_Machine.InstructionBases;

public class TwoRegistersWithDiscriminatorInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public Register Y => Arguments.MiddleNibble;
    public Nibble Discriminator => Arguments.LowNibble;

    public TwoRegistersWithDiscriminatorInstruction(Register X, Register Y, Nibble discriminator)
        : base(X, Y, discriminator)
    { }
}
