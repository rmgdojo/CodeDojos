namespace CHIP_8_Virtual_Machine.InstructionBases;

public class TwoRegistersInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public Register Y => Arguments.MiddleNibble;

    public override string Disassemble(VM vm)
    {
        return $"{Mnemonic.PadRight(5)} V{X}[{vm.V[X].ToString("X2")}],V{Y}[{vm.V[Y].ToString("X2")}]";
    }

    public TwoRegistersInstruction(Register X, Register Y, Nibble discriminator)
        : base(X, Y, discriminator)
    { }
}

