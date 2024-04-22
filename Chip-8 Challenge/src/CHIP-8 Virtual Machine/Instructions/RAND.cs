namespace CHIP_8_Virtual_Machine.Instructions;
    public class RAND : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public RAND(TwelveBit arguments)
        : base(arguments) { }
}