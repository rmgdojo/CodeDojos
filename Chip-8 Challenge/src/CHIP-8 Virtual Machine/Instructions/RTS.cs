namespace CHIP_8_Virtual_Machine.Instructions;
    public class RTS : AddressInstruction
{
    public override void Execute(VM vm)
    {
        vm.PC = vm.PopStack();
    }

    public RTS(TwelveBit address)
        : base(address) { }
}