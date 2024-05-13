namespace CHIP_8_Virtual_Machine.Instructions;
public class READ : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        for (Nybble i = 0; i < X; i++)
        {
            vm.V[i] = vm.RAM[(Tribble)(vm.I + i)];
        }
    }

    public READ(Register X)
        : base(X, 0x65) { }
}