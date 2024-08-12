using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class MOVED : RegisterInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] = vm.DelayTimer.GetCyclesRemaining();
    }

    public MOVED(Register X)
        : base(X, 0x07) { }
}