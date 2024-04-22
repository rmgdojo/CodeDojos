namespace CHIP_8_Virtual_Machine.Instructions;
    public class LOADD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public LOADD(TwelveBit arguments)
        : base(arguments) { }
}