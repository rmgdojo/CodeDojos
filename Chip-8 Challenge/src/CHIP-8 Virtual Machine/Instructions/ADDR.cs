using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class ADDR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        byte previousValue = vm.V[X];
        vm.V[X] += vm.V[Y];
        vm.SetFlag(previousValue > vm.V[X]);
    }

    public ADDR(Register X, Register Y)
        : base(X, Y, 0x4) { }
}