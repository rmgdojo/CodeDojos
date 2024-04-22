namespace CHIP_8_Virtual_Machine.Instructions;
    public class KEYD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public KEYD(TwelveBit arguments)
        : base(arguments) { }
}