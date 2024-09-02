using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class LOADD : RegisterInstruction
{
    public override void Execute(VM vm)
    {
        // Start the delay timer with the value in V[X]
        vm.DelayTimer.Start(vm.V[X]);
    }

    public LOADD(Register X)
        : base(X, 0x15) { }
}
