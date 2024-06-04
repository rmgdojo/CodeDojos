using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SKRE : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.V[X] == vm.V[Y])
        {
            vm.PC += 2;
        }
    }

    public SKRE(Register X, Register Y)
        : base(X, Y, 0x0) { }
}