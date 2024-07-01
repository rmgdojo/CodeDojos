using NUnit.Framework;
using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.Instructions;

namespace CHIP_8_Tests
{
    [TestFixture]
    public class InstructionTests
    {

        public VM VM { get; set; }

        [SetUp]
        public void Setup()
        {
            VM = new VM();
        }

        [Test]
        public void ADD()
        {
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SKUP(bool keyUp)
        {
            Tribble pc = VM.PC;

            if (keyUp)
            {
                VM.Keypad.WindowsKeyUp("1");
            }
            else
            {
                VM.Keypad.WindowsKeyDown("1");
            }

            SKUP instruction = new SKUP(0);
            instruction.Execute(VM);
            Tribble expectedPC = pc + (keyUp ? 2 : 0);

            Assert.That(VM.PC, Is.EqualTo(expectedPC));
        }

        [Test]
        public void BCD()
        {

            VM.V[1] = 123;

            BCD instruction = new BCD(1);
            instruction.Execute(VM);

            Tribble index = VM.I;

            Assert.That(VM.RAM[index], Is.EqualTo(1));
            Assert.That(VM.RAM[index + 1], Is.EqualTo(2));
            Assert.That(VM.RAM[index + 2], Is.EqualTo(3));
        }
    }
}