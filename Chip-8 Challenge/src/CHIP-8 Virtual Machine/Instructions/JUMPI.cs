namespace CHIP_8_Virtual_Machine.Instructions;
public class JUMPI : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PC = (Tribble)(Address + vm.I);
    }

    public JUMPI(Tribble address)
        : base(address) { }
}