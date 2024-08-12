using NUnit.Framework;
using CHIP_8_Virtual_Machine.Instructions;

namespace CHIP_8_Virtual_Machine.Tests.Instructions
{
    [TestFixture]
    public class SHRTests
    {
        private VM _vm;
        private SHR _shr;

        [SetUp]
        public void Setup()
        {
            _vm = new VM();
            _shr = new SHR(0, 1);
        }

        [Test]
        public void Execute_ShouldShiftRightByOne()
        {
            _vm.V[0] = 0b1010;
            _shr.Execute(_vm);

            Assert.That(_vm.V[1], Is.EqualTo(0b0101));
        }

        [Test]
        public void Execute_ShouldSetFlagIfLSBIsSet()
        {
            _vm.V[0] = 0b10111011;
            _shr.Execute(_vm);

            Assert.That(_vm.F == 1);
        }

        [Test]
        public void Execute_ShouldClearFlagIfLSBIsNotSet()
        {
            _vm.V[0] = 0b10101010;
            _shr.Execute(_vm);

            Assert.That(_vm.F == 0);
        }
    }
}
