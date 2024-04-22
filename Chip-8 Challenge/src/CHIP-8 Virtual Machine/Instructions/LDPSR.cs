namespace CHIP_8_Virtual_Machine.Instructions;
    public class LDPSR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public LDPSR(TwelveBit arguments)
        : base(arguments) { }
}