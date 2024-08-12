using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class CLR : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.Display.Clear();
    }

    public CLR(Tribble address)
        : base(address) { }
}