using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class LOAD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] = Value;
    }

    public LOAD(Register X, byte value)
        : base(X, value) { }
}