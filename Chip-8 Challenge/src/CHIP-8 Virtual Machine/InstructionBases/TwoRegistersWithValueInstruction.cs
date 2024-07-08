namespace CHIP_8_Virtual_Machine.InstructionBases;

public class TwoRegistersWithValueInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public Register Y => Arguments.MiddleNibble;
    public Nibble Value => Arguments.LowNibble;

    public TwoRegistersWithValueInstruction(Register X, Register Y, Nibble value)
        : base(X, Y, value)
    { }
}

