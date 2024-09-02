using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class NOP : AddressInstruction
{
    public override void Execute(VM vm)
    {
    }

    public NOP(Tribble address)
        : base(address) { }
}