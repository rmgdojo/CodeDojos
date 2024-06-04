namespace CHIP_8_Virtual_Machine.InstructionBases;

public class RegisterWithDiscriminatorInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public RegisterWithDiscriminatorInstruction(Register X, byte discriminator)
        : base(X, discriminator)
    { }
}
