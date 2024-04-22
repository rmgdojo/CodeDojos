namespace CHIP_8_Virtual_Machine.Instructions;
    public class SKE : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        if (vm.V[RegisterIndex] == Value)
        {
            vm.PC += 2;
        }
    }

    public SKE(TwelveBit arguments)
        : base(arguments) { }
}