using NUnit.Framework;
using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.Instructions;

namespace CHIP_8_Tests
{
    [TestFixture]
    public class InstructionTests
    {
        public string WINKEY_1 = "1";
        public Nibble KEYPAD_0 = 0;

        public VM VM { get; set; }
        
        public void KeyOp(bool down, string keyCode)
        {
            if (down)
            {
                VM.Keypad.WindowsKeyDown(keyCode);
            }
            else
            {
                VM.Keypad.WindowsKeyUp(keyCode);
            }
        }

        [SetUp]
        public void Setup()
        {
            VM = new VM();
        }

        [Test]
        public void ADD()
        {
        }

        [TestCase(true, ExpectedResult = 2)]
        [TestCase(false, ExpectedResult = 0)]
        public int SKUP(bool keyUp)
        {
            KeyOp(!keyUp, WINKEY_1);

            SKUP instruction = new SKUP(KEYPAD_0);
            instruction.Execute(VM);
            
            return VM.PC;
        }

        [TestCase(true, ExpectedResult = 2)]
        [TestCase(false, ExpectedResult = 0)]
        public int SKPR(bool keyDown)
        {
            KeyOp(keyDown, WINKEY_1);

            SKPR instruction = new SKPR(KEYPAD_0);
            instruction.Execute(VM);

            return VM.PC;
        }

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
}