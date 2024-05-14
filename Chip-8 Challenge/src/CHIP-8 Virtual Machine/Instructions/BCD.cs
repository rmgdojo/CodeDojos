namespace CHIP_8_Virtual_Machine.Instructions;
public class BCD : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public BCD(Register X)
        : base(X, 0x33) { }
}