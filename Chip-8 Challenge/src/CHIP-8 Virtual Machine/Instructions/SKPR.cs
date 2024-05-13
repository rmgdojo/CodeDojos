namespace CHIP_8_Virtual_Machine.Instructions;
public class SKPR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        throw new NotImplementedException();
    }

    public SKPR(Register X, byte value)
        : base(X, value) { }
}