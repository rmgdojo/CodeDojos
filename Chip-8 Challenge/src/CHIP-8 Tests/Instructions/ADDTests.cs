using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.Instructions;
using NUnit.Framework;

namespace CHIP_8_Tests.Instructions;

public class ADDTests : InstructionTestsBase
{
    [TestCase(0x00, 0, 0x01, 1)]
    [TestCase(0x00, 0, 0x7F, 127)]
    [TestCase(0x01, 1, 0x01, 2)]
    public void ADD(byte initialRegisterValue, int registerValue, byte byteValue, int expectedResult)
    {
        VM.V[(Register)registerValue] = initialRegisterValue;

        ADD add = new ADD((Register)registerValue, byteValue);

        add.Execute(VM);

        Assert.That(VM.V[(Register)registerValue], Is.EqualTo(expectedResult));
    }
}