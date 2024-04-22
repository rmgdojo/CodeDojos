namespace CHIP_8_Virtual_Machine.Instructions;
    public class OR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public OR(TwelveBit arguments)
        : base(arguments) { }
}