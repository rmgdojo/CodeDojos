namespace CHIP_8_Virtual_Machine.InstructionBases;

public class TwoRegistersWithValueInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public Register Y => Arguments.MiddleNibble;
    public Nibble Value => Arguments.LowNibble;

    public override string Disassemble(VM vm)
    {
        return $"{Mnemonic.PadRight(5)} V{X}[{vm.V[X].ToString("X2")}],V{Y}[{vm.V[Y].ToString("X2")}],{Value.ToHexString()}";
    }

    public TwoRegistersWithValueInstruction(Register X, Register Y, Nibble value)
        : base(X, Y, value)
    { }
}

