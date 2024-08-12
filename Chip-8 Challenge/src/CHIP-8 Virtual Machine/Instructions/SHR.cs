using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class SHR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        // shifts the value of register X right by 1 and stores the result in register Y
        // the value of bit 0 of register X is copied to register 0xF which is the CPU flag register
        byte vx = vm.V[X];
        vm.SetFlag(vx.GetBit(0)); // test bit 0 of the byte in register X and set the flag (register 0xF) to 1 or 0 if true or false
        vx = (byte)(vx >> 1); // shift the byte in register X to the right by 1
        vm.V[Y] = vx; // store the result in register Y
    }

    public SHR(Register X, Register Y)
        : base(X, Y, 0x6) { }
}