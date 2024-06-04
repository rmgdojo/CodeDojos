using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class ADD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] += Value;
    }

    public ADD(Register X, byte value)
        : base(X, value) { }
}