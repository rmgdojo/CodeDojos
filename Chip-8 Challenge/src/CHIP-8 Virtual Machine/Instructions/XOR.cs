namespace CHIP_8_Virtual_Machine.Instructions;
    public class XOR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public XOR(TwelveBit arguments)
        : base(arguments) { }
}