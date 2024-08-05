namespace CHIP_8_Virtual_Machine.InstructionBases;

public class RegisterInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public RegisterInstruction(Register X, byte discriminator)
        : base(X, discriminator)
    { }
}
