namespace CHIP_8_Virtual_Machine.Instructions;
public class CLR : AddressInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public CLR(TwelveBit address)
        : base(address) { }
}