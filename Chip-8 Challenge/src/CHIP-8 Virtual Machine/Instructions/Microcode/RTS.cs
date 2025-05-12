using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class RTS : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PC = vm.Stack.Pop();
    }

    public RTS(Tribble address)
        : base(address) { }
}