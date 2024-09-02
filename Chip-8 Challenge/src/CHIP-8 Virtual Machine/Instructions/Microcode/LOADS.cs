using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class LOADS : RegisterInstruction
{
    public override void Execute(VM vm)
    {
        vm.SoundTimer.Start(vm.V[X]);
    }

    public LOADS(Register X)
        : base(X, 0x18) { }
}
