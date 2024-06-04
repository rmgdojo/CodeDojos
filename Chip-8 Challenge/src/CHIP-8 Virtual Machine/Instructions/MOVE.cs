using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class MOVE : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[Y] = vm.V[X];
    }

    public MOVE(Register X, Register Y)
        : base(X, Y, 0x0) { }
}