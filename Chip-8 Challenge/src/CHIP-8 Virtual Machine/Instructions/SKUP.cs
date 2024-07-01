using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SKUP : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        if (!vm.Keypad[X])
        {
            vm.PC += 2;
        }
    }

    public SKUP(Register X)
        : base(X, 0xA1) { }
}