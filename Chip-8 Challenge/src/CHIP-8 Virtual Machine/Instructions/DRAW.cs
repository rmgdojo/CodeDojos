namespace CHIP_8_Virtual_Machine.Instructions;
    public class DRAW : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public DRAW(TwelveBit arguments)
        : base(arguments) { }
}