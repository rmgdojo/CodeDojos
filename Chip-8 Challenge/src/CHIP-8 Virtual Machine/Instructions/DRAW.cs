using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class DRAW : TwoRegistersWithDiscriminatorInstruction
{
    public override void Execute(VM vm)
    {
        byte[] bytes = vm.RAM.GetBytes(vm.I, Discriminator);
        vm.F = (byte)(vm.Display.DisplaySprite(X, Y, bytes) ? 1 : 0);
    }

    public DRAW(Register X, Register Y, Nibble n)
        : base(X, Y, n) { }
}