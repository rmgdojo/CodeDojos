namespace CHIP_8_Virtual_Machine.Instructions;
public class STOR : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        for (Nybble i = 0; i < X; i++)
        {
            vm.RAM[(Tribble)(vm.I + i)] = vm.V[i];
        }
    }

    public STOR(Register X)
        : base(X, 0x55) { }
}