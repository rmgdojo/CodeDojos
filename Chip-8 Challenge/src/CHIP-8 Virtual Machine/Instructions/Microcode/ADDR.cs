using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class ADDR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        int result = vm.V[X] + vm.V[Y];
        bool overflow = result > 0xFF;

        vm.V[X] = (byte)result;
        vm.F = overflow;
    }

    public ADDR(Register X, Register Y)
        : base(X, Y, 0x4) { }
}