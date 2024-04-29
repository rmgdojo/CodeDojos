namespace CHIP_8_Virtual_Machine.Instructions;
public class SHL : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SHL(TwelveBit arguments)
        : base(arguments) { }
}