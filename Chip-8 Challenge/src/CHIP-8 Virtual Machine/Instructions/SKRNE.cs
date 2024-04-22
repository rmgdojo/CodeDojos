namespace CHIP_8_Virtual_Machine.Instructions;
    public class SKRNE : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SKRNE(TwelveBit arguments)
        : base(arguments) { }
}