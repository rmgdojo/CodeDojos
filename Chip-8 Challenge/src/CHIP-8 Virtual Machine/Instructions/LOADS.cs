namespace CHIP_8_Virtual_Machine.Instructions;
public class LOADS : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public LOADS(TwelveBit arguments)
        : base(arguments) { }
}