using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class ADDR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        byte previousValue = vm.V[X];
        vm.V[X] += vm.V[Y];
        vm.F = (byte)((previousValue > vm.V[X])?1:0);
    }

    public ADDR(Register X, Register Y)
        : base(X, Y, 0x4) { }
}