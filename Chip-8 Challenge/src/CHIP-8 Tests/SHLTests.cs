using NUnit.Framework;
using CHIP_8_Virtual_Machine.Instructions;

namespace CHIP_8_Virtual_Machine.Tests.Instructions
{
    [TestFixture]
    public class SHLTests
    {
        private VM _vm;
        private SHL _shl;

        [SetUp]
        public void Setup()
        {
            _vm = new VM();
            _shl = new SHL(0, 1);
        }

        [Test]
        public void Execute_ShouldShiftLeftByOne()
        {
            _vm.V[0] = 0b0101;
            _shl.Execute(_vm);

            Assert.That(_vm.V[1], Is.EqualTo(0b1010));
        }

        [Test]
        public void Execute_ShouldSetFlagIfMSBIsSet()
        {
            _vm.V[0] = 0b10111011;
            _shl.Execute(_vm);

            Assert.That(_vm.F == 1);
        }

        [Test]
        public void Execute_ShouldClearFlagIfMSBIsNotSet()
        {
            _vm.V[0] = 0b01010101;
            _shl.Execute(_vm);

            Assert.That(_vm.F == 0);
        }
    }
}
