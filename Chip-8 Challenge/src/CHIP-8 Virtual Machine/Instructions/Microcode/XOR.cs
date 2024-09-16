using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class XOR : TwoRegistersInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[X] = (byte)(vm.V[X] ^ vm.V[Y]);
    }

    public XOR(Register X, Register Y)
        : base(X, Y, 0x3) { }
}