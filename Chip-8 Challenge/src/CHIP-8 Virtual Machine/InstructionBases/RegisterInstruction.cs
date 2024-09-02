namespace CHIP_8_Virtual_Machine.InstructionBases;

public class RegisterInstruction : Instruction
{
    public Register X => Arguments.HighNibble;

    public override string Disassemble(VM vm)
    {
        return $"{Mnemonic.PadRight(5)} V{X}[{vm.V[X].ToString("X2")}]";
    }

    public RegisterInstruction(Register X, byte discriminator)
        : base(X, discriminator)
    { }
}
