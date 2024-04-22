namespace CHIP_8_Virtual_Machine.Instructions;
    public class CALL : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PushStack(vm.PC);
        vm.PC = Address;
    }

    public CALL(TwelveBit address)
        : base(address) { }
}