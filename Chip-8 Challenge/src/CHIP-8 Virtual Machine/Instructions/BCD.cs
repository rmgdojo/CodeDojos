using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class BCD : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        byte value = vm.V[X];
        Tribble index = vm.I;

        vm.RAM[index] = (byte)((value / 100) % 10);
        vm.RAM[index + 1] = (byte)((value / 10) % 10);
        vm.RAM[index + 2] = (byte)(value % 10);
    }

    public BCD(Register X)
        : base(X, 0x33) { }
}