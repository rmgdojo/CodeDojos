using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SUBR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[Y] -= vm.V[X];
        vm.F = (byte)(vm.V[Y] > vm.V[X] ? 1 : 0);
    }

    public SUBR(Register X, Register Y)
        : base(X, Y, 0x7) { }
}