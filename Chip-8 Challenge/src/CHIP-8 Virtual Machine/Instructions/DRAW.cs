using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;

public class DRAW : TwoRegistersWithValueInstruction
{
    public override void Execute(VM vm)
    {
        byte[] bytes = vm.RAM.GetBytes(vm.I, Value);
        bool erased = vm.Display.DisplaySprite(vm.V[X], vm.V[Y], bytes);
        vm.SetFlag(erased);
    }

    public DRAW(Register X, Register Y, Nibble n)
        : base(X, Y, n) { }
}