using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SUBR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        int result = vm.V[Y] - vm.V[X];
        bool underflow = result < 0;

        vm.V[X] = (byte)result;
        vm.SetFlag(!underflow);
    }

    public SUBR(Register X, Register Y)
        : base(X, Y, 0x7) { }
}