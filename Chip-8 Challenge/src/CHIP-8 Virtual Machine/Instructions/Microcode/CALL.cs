using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class CALL : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PushStack(vm.PC);
        vm.PC = Address;
    }

    public CALL(Tribble address)
        : base(address) { }
}