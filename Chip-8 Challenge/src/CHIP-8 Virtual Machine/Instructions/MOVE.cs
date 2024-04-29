namespace CHIP_8_Virtual_Machine.Instructions;
    public class MOVE : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[RegisterIndex2] = vm.V[RegisterIndex1];
    }

    public MOVE(TwelveBit arguments)
        : base(arguments) { }
}