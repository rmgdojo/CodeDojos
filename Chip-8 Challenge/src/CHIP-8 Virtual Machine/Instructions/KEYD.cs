namespace CHIP_8_Virtual_Machine.Instructions;
public class KEYD : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public KEYD(Register X)
        : base(X, 0x0A) { }
}