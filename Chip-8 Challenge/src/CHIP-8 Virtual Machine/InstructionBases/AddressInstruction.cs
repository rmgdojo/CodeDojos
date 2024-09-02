namespace CHIP_8_Virtual_Machine.InstructionBases;

public class AddressInstruction : Instruction
{
    public Tribble Address => Arguments;

    public override string Disassemble(VM vm)
    {
        return $"{Mnemonic.PadRight(5)} {Address.ToHexString()}";
    }

    public AddressInstruction(Tribble address)
        : base(address)
    { }
}
