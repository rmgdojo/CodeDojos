using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class ADDI : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        vm.I += vm.V[X];
    }

    public ADDI(Register X)
        : base(X, 0x1E) { }
}