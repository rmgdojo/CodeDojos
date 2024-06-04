using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class OR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[Y] = (byte)(vm.V[X] | vm.V[Y]) ;
    }

    public OR(Register X, Register Y)
        : base(X, Y, 0x1) { }
}