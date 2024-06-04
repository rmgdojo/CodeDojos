using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SUB : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] -= vm.V[Y];
        vm.F = (byte)(vm.V[X] > vm.V[Y] ? 1 : 0);
    }

    public SUB(Register X, Register Y)
        : base(X, Y, 0x5) { }
}