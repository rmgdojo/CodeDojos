using CHIP_8_Virtual_Machine.Instructions;
using NUnit.Framework;

namespace CHIP_8_Tests.Instructions;

public class BCDTests : InstructionTestsBase
{
    [Test]
    public void BCD()
    {
        VM.V[1] = 123;

        BCD instruction = new BCD(1);
        instruction.Execute(VM);

        Assert.That(VM.RAM[VM.I], Is.EqualTo(1));
        Assert.That(VM.RAM[VM.I + 1], Is.EqualTo(2));
        Assert.That(VM.RAM[VM.I + 2], Is.EqualTo(3));
    }
}