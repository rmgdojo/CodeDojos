using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SKPR : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.Keypad[X])
        {
            vm.PC += 2;
        }
    }

    public SKPR(Register X, byte value)
        : base(X, value) { }
}