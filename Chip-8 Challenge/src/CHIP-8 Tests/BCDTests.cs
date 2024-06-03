using NUnit.Framework;

namespace CHIP_8_Virtual_Machine.Instructions.Tests
{
    public class BCDTests
    {
        [Test]
        public void Execute_ShouldConvertValueToBCD()
        {
            VM vm = new VM();
            vm.V[1] = 123;

            BCD instruction = new BCD(1);
            instruction.Execute(vm);

            Tribble index = vm.I;

            Assert.That(vm.RAM[index], Is.EqualTo(1));
            Assert.That(vm.RAM[index + 1], Is.EqualTo(2));
            Assert.That(vm.RAM[index + 2], Is.EqualTo(3));
        }
    }
}
