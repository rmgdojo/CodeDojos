namespace CHIP_8_Virtual_Machine.Instructions;
    public class ADD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[RegisterIndex] += Value;
    }

    public ADD(TwelveBit arguments)
        : base(arguments) { }
}