using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class JUMPI : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PC = Address + vm.I;
    }

    public JUMPI(Tribble address)
        : base(address) { }
}