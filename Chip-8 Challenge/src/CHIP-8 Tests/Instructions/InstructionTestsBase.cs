using NUnit.Framework;
using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.InstructionBases;

namespace CHIP_8_Tests.Instructions
{
    [TestFixture]
    public abstract class InstructionTestsBase
    {
        public string WINKEY_1 = "D1";
        public Nibble KEYPAD_0 = 0;

        public VM VM { get; set; }

        public void KeyOp(bool down, string keyCode)
        {
            if (down)
            {
                VM.Keypad.KeyDown(keyCode);
            }
            else
            {
                VM.Keypad.KeyUp(keyCode);
            }
        }

        [SetUp]
        public void Setup()
        {
            VM = new VM(new WindowsKeypadMap());
        }
    }
}