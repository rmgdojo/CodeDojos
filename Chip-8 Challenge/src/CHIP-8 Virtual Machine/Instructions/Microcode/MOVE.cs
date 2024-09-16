using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class MOVE : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] = vm.V[Y];
    }

    public MOVE(Register X, Register Y)
        : base(X, Y, 0x0) { }
}