namespace CHIP_8_Virtual_Machine.Instructions;
    public class SKRE : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SKRE(TwelveBit arguments)
        : base(arguments) { }
}