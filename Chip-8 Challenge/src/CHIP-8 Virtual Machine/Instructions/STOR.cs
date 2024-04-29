namespace CHIP_8_Virtual_Machine.Instructions;
public class STOR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public STOR(TwelveBit arguments)
        : base(arguments) { }
}