using NUnit.Framework;
using CHIP_8_Virtual_Machine.Instructions;
using CHIP_8_Virtual_Machine;

namespace CHIP_8_Tests.Instructions
{
    [TestFixture]
    public class SHLTests : InstructionTestsBase
    {
        private SHL _shl;

        [SetUp]
        public void Setup()
        {
            _shl = new SHL(0, 1); // register X = V0, Y = V1
        }

        [Test]
        public void Execute_ShouldShiftLeftByOne()
        {
            VM.V[0] = 0b0101; // has bit 0 set
            _shl.Execute(VM);

            Assert.That(VM.V[1], Is.EqualTo(0b1010)); // shifted left by 1
        }

        [Test]
        public void Execute_ShouldSetFlagIfMSBIsSet()
        {
            VM.V[0] = 0b10111011; // bit 7 is set
            _shl.Execute(VM);

            Assert.That(VM.F == 1);
        }

        [Test]
        public void Execute_ShouldClearFlagIfMSBIsNotSet()
        {
            VM.V[0] = 0b01010101; // bit 7 is not set
            _shl.Execute(VM);

            Assert.That(VM.F == 0);
        }
    }
}
