using CHIP_8_Virtual_Machine.Instructions;
using CHIP_8_Virtual_Machine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Tests.Instructions
{
    [TestFixture]
    public class SUBTests : InstructionTestsBase
    {
        private SUB _sub;

        [TestCase(0, 1, 127, 64, 63)]
        [TestCase(15, 5, 127, 64, 63)]
        [TestCase(1, 1, 127, 64, 0)]
        [TestCase(1, 3, 64, 64, 0)]
        public void VX_EqualsVXMinusVY(int x, int y, byte xValue, byte yValue, int expectedVx)
        {
            _sub = new SUB((Register)x, (Register)y);
            VM.V[_sub.X] = xValue;
            VM.V[_sub.Y] = yValue;

            _sub.Execute(VM);

            Assert.That(VM.V[_sub.X], Is.EqualTo(expectedVx));
        }


        [TestCase(0, 1, 64, 127, 0)]
        [TestCase(2, 3, 127, 64, 1)]
        public void VF_SetUnderflow(int x, int y, byte xValue, byte yValue, int isUnderflow)
        {
            _sub = new SUB((Register)x, (Register)y);
            VM.V[_sub.X] = xValue;
            VM.V[_sub.Y] = yValue;

            _sub.Execute(VM);

            Assert.That(VM.V[0xF], Is.EqualTo(isUnderflow));
        }
    }
}
