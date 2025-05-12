using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SUB : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        int result = vm.V[X] - vm.V[Y];
        bool underflow = result < 0;

        vm.F = !underflow;
        vm.V[X] = (byte)result;
    }

    public SUB(Register X, Register Y)
        : base(X, Y, 0x5) { }
}