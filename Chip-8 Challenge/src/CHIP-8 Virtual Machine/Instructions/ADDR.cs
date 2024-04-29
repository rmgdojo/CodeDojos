namespace CHIP_8_Virtual_Machine.Instructions;
    public class ADDR : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        byte previousValue = vm.V[RegisterIndex1];
        vm.V[RegisterIndex1] += vm.V[RegisterIndex2];
        vm.V.F = (byte)((previousValue > vm.V[RegisterIndex1])?1:0);
    }

    public ADDR(TwelveBit arguments)
        : base(arguments) { }
}