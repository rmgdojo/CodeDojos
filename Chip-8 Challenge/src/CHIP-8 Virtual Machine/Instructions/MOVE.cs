namespace CHIP_8_Virtual_Machine.Instructions;
    public class MOVE : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public MOVE(TwelveBit arguments)
        : base(arguments) { }
}