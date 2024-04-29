namespace CHIP_8_Virtual_Machine.Instructions;
public class XOR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[RegisterIndex2] = (byte)(vm.V[RegisterIndex1] | vm.V[RegisterIndex2]);
    }

    public XOR(TwelveBit arguments)
        : base(arguments) { }
}