using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class LOADI : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.I = Address;
    }

    public LOADI(Tribble address)
        : base(address) { }
}