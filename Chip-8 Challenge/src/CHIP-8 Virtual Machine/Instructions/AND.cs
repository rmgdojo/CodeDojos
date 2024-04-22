namespace CHIP_8_Virtual_Machine.Instructions;
    public class AND : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public AND(TwelveBit arguments)
        : base(arguments) { }
}