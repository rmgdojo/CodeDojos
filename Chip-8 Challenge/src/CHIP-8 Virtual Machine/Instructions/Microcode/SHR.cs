using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SHR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        // shifts the value of register X right by 1 and stores the result in register Y
        // the value of bit 0 of register X is copied to register 0xF which is the CPU flag register
        byte vx = vm.V[X];
        vm.V[Y] = (byte)(vx >> 1);
        vm.F = vx.GetBit(0);
    }

    public SHR(Register X, Register Y)
        : base(X, Y, 0x6) { }
}