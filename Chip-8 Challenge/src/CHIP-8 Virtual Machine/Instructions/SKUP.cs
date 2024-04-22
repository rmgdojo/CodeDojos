namespace CHIP_8_Virtual_Machine.Instructions;
    public class SKUP : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SKUP(TwelveBit arguments)
        : base(arguments) { }
}