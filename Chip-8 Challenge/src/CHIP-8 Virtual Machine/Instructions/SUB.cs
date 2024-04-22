namespace CHIP_8_Virtual_Machine.Instructions;
    public class SUB : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SUB(TwelveBit arguments)
        : base(arguments) { }
}