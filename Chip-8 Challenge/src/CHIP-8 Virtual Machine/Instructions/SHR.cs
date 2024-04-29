namespace CHIP_8_Virtual_Machine.Instructions;
public class SHR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SHR(TwelveBit arguments)
        : base(arguments) { }
}