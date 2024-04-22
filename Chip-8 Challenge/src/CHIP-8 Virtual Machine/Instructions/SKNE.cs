namespace CHIP_8_Virtual_Machine.Instructions;
    public class SKNE : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SKNE(TwelveBit arguments)
        : base(arguments) { }
}