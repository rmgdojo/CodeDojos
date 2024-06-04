namespace CHIP_8_Virtual_Machine.InstructionBases;

public class RegisterWithValueInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public byte Value => Arguments.LowByte;

    public RegisterWithValueInstruction(Register X, byte value)
        : base(X, value)
    { }
}
