namespace CHIP_8_Virtual_Machine.Instructions;
public class ADDI : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public ADDI(TwelveBit arguments)
        : base(arguments) { }
}