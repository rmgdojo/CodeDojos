using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecimalToRoman
{
    public static class RomanNumerals
    {
        private static Dictionary<int, string> _romanNumberDictionary = new Dictionary<int, string>()
        {
            {1000, "M"},
            { 900, "CM"},
            { 500, "D"},
            { 400, "CD"},
            { 100, "C"},
            { 90, "XC"},
            { 50, "L"},
            { 40, "XL"},
            { 10, "X"},
            { 9, "IX"},
            { 5, "V"},
            { 4, "IV"},
            { 1, "I"}
        };

        public static string ToRoman(uint input)
        {
            if (input > 3999) throw new ArgumentException("Values greater than 3999 are not allowed.");
            
            StringBuilder romanNumerals = new();
            foreach (var pair in _romanNumberDictionary)
            {
                while (input >= pair.Key)
                {
                    romanNumerals.Append(pair.Value);
                    input -= (uint)pair.Key;
                }
            }

            return romanNumerals.ToString();
        }
    }
}
