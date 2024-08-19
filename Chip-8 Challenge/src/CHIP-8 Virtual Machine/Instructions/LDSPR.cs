using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Virtual_Machine.Instructions;
public class LDSPR : RegisterInstruction
{
    public override void Execute(VM vm)
    {
        char c = (char)(((byte)X) + '0');
        vm.I = vm.SystemFont.AddressOf(c);
    }

    public LDSPR(Register X)
        : base(X, 0x29) { }
}