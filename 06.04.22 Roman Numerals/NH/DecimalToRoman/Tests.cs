using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DecimalToRoman
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(1987, "MCMLXXXVII")]
        [TestCase(2019, "MMXIX")]
        public void GivenIntReturnsRoman(uint input, string expected)
        {
            string actual = RomanNumerals.ToRoman(input);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
