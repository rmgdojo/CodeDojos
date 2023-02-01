using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardValidator
{
    internal class LuhnCheck
    {
        public bool IsValid { get; private set; }

        private bool CheckIsValid(string cardNumber)
        {
            int[] numbers = new int[cardNumber.Length];
            int index = cardNumber.Length - 1;

            do 
            {
                numbers[index] = parse(cardNumber[index]);
                index--;
                
                numbers[index] = parse(cardNumber[index]) * 2;
                if (numbers[index] > 9) numbers[index] -= 9;
                index--;
            } 
            while (index >= 0);

            return numbers.Sum() % 10 == 0;

            int parse(char c)
            {
                return (int)c - 48;
            }
        }

        internal LuhnCheck(string cardNumber)
        {
            IsValid = CheckIsValid(cardNumber);
        }
    }
}
