using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.Instructions;
using NUnit.Framework;

namespace CHIP_8_Tests.Instructions
{
    [TestFixture]
    public class SHRTests : InstructionTestsBase
    {
        private SHR _shr;

        [SetUp]
        public void Setup()
        {
            _shr = new SHR(0, 1); // register X = V0, Y = V1
        }

        [Test]
        public void Execute_ShouldShiftRightByOne()
        {
            VM.V[0] = 0b1010; // has bit 0 set
            _shr.Execute(VM);

            Assert.That(VM.V[1], Is.EqualTo(0b0101));
        }

        [Test]
        public void Execute_ShouldSetFlagIfLSBIsSet()
        {
            VM.V[0] = 0b10111011; // bit 0 is set
            _shr.Execute(VM);

            Assert.That(VM.F, Is.True);
        }

        [Test]
        public void Execute_ShouldClearFlagIfLSBIsNotSet()
        {
            VM.V[0] = 0b10101010; // bit 0 is not set
            _shr.Execute(VM);

            Assert.That(VM.F, Is.False);
        }
    }
}
