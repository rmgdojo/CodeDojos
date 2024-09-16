using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SUB : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        vm.SetFlag(vm.V[X] > vm.V[Y]);
        vm.V[X] -= vm.V[Y];
    }

    public SUB(Register X, Register Y)
        : base(X, Y, 0x5) { }
}