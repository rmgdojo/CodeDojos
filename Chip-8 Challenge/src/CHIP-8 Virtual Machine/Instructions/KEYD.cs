using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class KEYD : RegisterWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] = vm.Keypad.WaitForKeyPress();
    }

    public KEYD(Register X)
        : base(X, 0x0A) { }
}