namespace CardValidator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] numbers = new string[] {
                "5555555555554444", // OK
                "123456",
                "5105105105105100", // OK
                "4321123443211234",
                "4111111111111111", // OK
                "9876123455550000",
                "4012888888881881", // OK
                "4444171300111100",
                "378282246310005", // OK
                "000000000000000",
                "6011111111111117" // OK
            };

            foreach (string number in numbers)
            {
                try
                {
                    Validator validator = new Validator(number);
                    Console.WriteLine($"{number}: {(validator.IsValid ? "VALID" : "INVALID")}");
                }
                catch
                {
                    Console.WriteLine($"{number}: INVALID");
                }
            }

            Console.ReadLine();
        }
    }
}