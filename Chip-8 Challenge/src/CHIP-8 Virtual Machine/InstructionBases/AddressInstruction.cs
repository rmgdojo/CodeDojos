namespace CHIP_8_Virtual_Machine.InstructionBases;

public class AddressInstruction : Instruction
{
    public Tribble Address => Arguments;

    public AddressInstruction(Tribble address)
        : base(address)
    { }
}
