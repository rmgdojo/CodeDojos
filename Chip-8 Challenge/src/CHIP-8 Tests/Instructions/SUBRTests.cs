using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.Instructions;
using NUnit.Framework;

namespace CHIP_8_Tests.Instructions;

[TestFixture]
public class SUBRTests : InstructionTestsBase
{

    [TestCase(0, 1, 64, 127, 63)]
    [TestCase(15, 5, 64, 127, 63)]
    [TestCase(1, 1, 64, 127, 0)]
    [TestCase(1, 3, 64, 64, 0)]
    public void VX_EqualsVYMinusMX(int x, int y, byte xValue, byte yValue, int expectedVx)
    {
        var subr = new SUBR((Register)x, (Register)y);
        VM.V[subr.X] = xValue;
        VM.V[subr.Y] = yValue;

        subr.Execute(VM);

        Assert.That(VM.V[subr.X], Is.EqualTo(expectedVx));
    }


    [TestCase(0, 1, 64, 127, 1)]
    [TestCase(2, 3, 127, 64,  0)]
    public void VF_SetUnderflow(int x, int y, byte xValue, byte yValue, int isUnderflow)
    {
        var subr = new SUBR((Register)x, (Register)y);
        VM.V[subr.X] = xValue;
        VM.V[subr.Y] = yValue;

        subr.Execute(VM);

        Assert.That(VM.V[0xF], Is.EqualTo(isUnderflow));
    }
}

