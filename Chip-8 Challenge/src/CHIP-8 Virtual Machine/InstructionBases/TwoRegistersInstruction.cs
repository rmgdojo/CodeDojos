namespace CHIP_8_Virtual_Machine.InstructionBases;

public class TwoRegistersInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public Register Y => Arguments.MiddleNibble;

    public TwoRegistersInstruction(Register X, Register Y, Nibble discriminator)
        : base(X, Y, discriminator)
    { }
}

