namespace CHIP_8_Virtual_Machine.Instructions;
public class NOP : AddressInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public NOP(TwelveBit address)
        : base(address) { }
}