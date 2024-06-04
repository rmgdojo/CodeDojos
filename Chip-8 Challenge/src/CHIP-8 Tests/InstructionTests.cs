using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.Instructions;

namespace CHIP_8_Tests
{
    [TestFixture]
    public class InstructionTests
    {
        [Test]
        public void ADD()
        {
        }

        [Test]
        public void BCD()
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
