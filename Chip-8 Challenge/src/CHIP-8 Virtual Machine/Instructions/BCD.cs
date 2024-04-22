namespace CHIP_8_Virtual_Machine.Instructions;
    public class BCD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public BCD(TwelveBit arguments)
        : base(arguments) { }
}