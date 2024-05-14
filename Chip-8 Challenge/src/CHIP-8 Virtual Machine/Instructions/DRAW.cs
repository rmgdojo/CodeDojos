namespace CHIP_8_Virtual_Machine.Instructions;
public class DRAW : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public DRAW(Register X, Register Y, Nybble n)
        : base(X, Y, n) { }
}