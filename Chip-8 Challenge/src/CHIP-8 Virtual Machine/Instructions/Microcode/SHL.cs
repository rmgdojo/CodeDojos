using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SHL : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        // shifts the byte in register X to the left by 1, and stores the result in register Y
        // the value of bit 7 of register X is copied to register 0xF which is the CPU flag register
        byte vx = vm.V[X];
        vm.V[Y] = (byte)(vx << 1);
        vm.F = vx.GetBit(7);
    }

    public SHL(Register X, Register Y)
        : base(X, Y, 0xE) { }
}