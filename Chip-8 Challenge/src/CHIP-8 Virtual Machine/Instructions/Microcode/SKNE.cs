using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SKNE : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.V[X] != Value)
        {
            vm.PC += 2;
        }
    }

    public SKNE(Register X, byte value)
        : base(X, value) { }
}