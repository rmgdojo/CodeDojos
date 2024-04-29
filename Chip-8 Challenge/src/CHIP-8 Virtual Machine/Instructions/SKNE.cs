namespace CHIP_8_Virtual_Machine.Instructions;
    public class SKNE : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.V[RegisterIndex] != Value)
        {
            vm.PC += 2;
        }
    }

    public SKNE(TwelveBit arguments)
        : base(arguments) { }
}