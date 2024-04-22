namespace CHIP_8_Virtual_Machine.Instructions;
    public class READ : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public READ(TwelveBit arguments)
        : base(arguments) { }
}