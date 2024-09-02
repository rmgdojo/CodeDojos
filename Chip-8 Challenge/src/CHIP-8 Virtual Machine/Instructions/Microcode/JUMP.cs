using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class JUMP : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PC = Address;
    }

    public JUMP(Tribble address)
        : base(address) { }
}