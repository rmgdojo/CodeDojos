using CHIP_8_Virtual_Machine.Instructions;
using NUnit.Framework;

namespace CHIP_8_Tests.Instructions;

public class SKUPTests : InstructionTestsBase
{
    [TestCase(true, ExpectedResult = 2)]
    [TestCase(false, ExpectedResult = 0)]
    public int SKUP(bool keyUp)
    {
        KeyOp(!keyUp, WINKEY_1);

        SKUP instruction = new SKUP(KEYPAD_0);
        instruction.Execute(VM);

        return VM.PC;
    }
}