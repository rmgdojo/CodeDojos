namespace CHIP_8_Virtual_Machine.Instructions;
public class SKRE : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.V[RegisterIndex1] == vm.V[RegisterIndex2])
        {
            vm.PC += 2;
        }
    }

    public SKRE(TwelveBit arguments)
        : base(arguments) { }
}