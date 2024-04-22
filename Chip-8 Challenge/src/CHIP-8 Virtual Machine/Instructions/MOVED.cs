namespace CHIP_8_Virtual_Machine.Instructions;
    public class MOVED : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public MOVED(TwelveBit arguments)
        : base(arguments) { }
}