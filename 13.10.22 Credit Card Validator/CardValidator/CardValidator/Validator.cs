using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardValidator
{
    public class Validator
    {
        public bool IsValid { get; private set; }
        
        internal bool IsValidInputString(string cardNumber, out string cleanedNumber)
        {
            cleanedNumber = cardNumber.Replace("-", "");
            if (!long.TryParse(cleanedNumber, out long numeric)) return false;
            if (cleanedNumber.Length > 16 || cleanedNumber.Length < 13 || cleanedNumber.Length == 14) return false;
            return true;
        }

        public Validator(string cardNumber)
        {
            if (!IsValidInputString(cardNumber, out string cleanedNumber)) throw new ArgumentException("Invalid card string");

            CreditCardNumberCheck numberCheck = new CreditCardNumberCheck(cleanedNumber);
            LuhnCheck luhnCheck = new LuhnCheck(cleanedNumber);

            IsValid = numberCheck.IsValid && luhnCheck.IsValid;
        }
    }
}
