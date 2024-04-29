namespace CHIP_8_Virtual_Machine.Instructions;
public class SKPR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SKPR(TwelveBit arguments)
        : base(arguments) { }
}