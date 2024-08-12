using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SUBR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[Y] -= vm.V[X];
        vm.SetFlag(vm.V[Y] > vm.V[X]);
    }

    public SUBR(Register X, Register Y)
        : base(X, Y, 0x7) { }
}