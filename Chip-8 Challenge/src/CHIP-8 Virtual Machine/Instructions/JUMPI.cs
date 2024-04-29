namespace CHIP_8_Virtual_Machine.Instructions;
public class JUMPI : AddressInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public JUMPI(TwelveBit address)
        : base(address) { }
}