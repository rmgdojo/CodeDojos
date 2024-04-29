namespace CHIP_8_Virtual_Machine.Instructions;
public class LOADI : AddressInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public LOADI(TwelveBit address)
        : base(address) { }
}