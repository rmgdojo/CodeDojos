namespace CHIP_8_Virtual_Machine.Instructions;
public class SUBR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SUBR(TwelveBit arguments)
        : base(arguments) { }
}