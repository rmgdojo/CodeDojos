namespace CHIP_8_Virtual_Machine.Instructions;
    public class ADDR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public ADDR(TwelveBit arguments)
        : base(arguments) { }
}