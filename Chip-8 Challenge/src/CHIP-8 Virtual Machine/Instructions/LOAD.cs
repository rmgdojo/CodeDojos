namespace CHIP_8_Virtual_Machine.Instructions;
    public class LOAD : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        vm.V[RegisterIndex] = Value;
    }

    public LOAD(TwelveBit arguments)
        : base(arguments) { }
}