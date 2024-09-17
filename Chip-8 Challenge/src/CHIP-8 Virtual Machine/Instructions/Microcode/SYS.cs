using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SYS : AddressInstruction
{
    public override void Execute(VM vm)
    {
    }

    public SYS(Tribble address)
        : base(address) { }
}