using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SKPR : RegisterInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.Keypad[X])
        {
            vm.PC += 2;
        }
    }

    public SKPR(Register X)
        : base(X, 0x9E) { }

}