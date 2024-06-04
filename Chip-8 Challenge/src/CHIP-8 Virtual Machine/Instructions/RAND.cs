using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class RAND : RegisterWithValueInstruction
{
    public override void Execute(VM vm)
    {
        Random random = new(DateTime.Now.Millisecond);
        vm.V[X] = (byte)random.Next(0, Value);
    }

    public RAND(Register X, byte value)
        : base(X, value) { }
}