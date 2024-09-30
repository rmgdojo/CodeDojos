using CHIP_8_Virtual_Machine.Instructions;
using NUnit.Framework;

namespace CHIP_8_Tests.Instructions;

public class SKPRTests : InstructionTestsBase
{
    [TestCase(true, ExpectedResult = 2)]
    [TestCase(false, ExpectedResult = 0)]
    public int SKPR(bool keyDown)
    {
        KeyOp(keyDown, WINKEY_1);

        SKPR instruction = new SKPR(KEYPAD_0);
        instruction.Execute(VM);

        return VM.PC;
    }
}