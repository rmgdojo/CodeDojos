using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardValidator
{
    internal class CreditCardNumberCheck
    {
        public bool IsValid { get; private set; }

        private bool CheckIfValid(string cardNumber)
        {
            return cardNumber switch
            {
                var c when (c.StartsWith("34") || c.StartsWith("37")) && c.Length == 15 => true, // AMEX
                var c when (c.StartsWith("6011") && c.Length == 16) => true, // DISCOVER
                var c when (c.StartsWith("5") && (c[1] > '0' && c[1] < '6') && c.Length == 16) => true, // MC
                var c when (c.StartsWith("4") && (c.Length == 13 || c.Length == 16)) => true, // VISA
            };
        }

        internal CreditCardNumberCheck(string cardNumber)
        {
            IsValid = CheckIfValid(cardNumber);
        }
    }
}
