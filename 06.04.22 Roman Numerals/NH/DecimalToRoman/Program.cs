// See https://aka.ms/new-console-template for more information
using DecimalToRoman;

while (true)
{
    Console.Write("Enter integer number: ");
    string value = Console.ReadLine();

    if (uint.TryParse(value, out uint integer))
    {
        try
        {
            Console.WriteLine($"Roman numerals are: { RomanNumerals.ToRoman(integer) }\n");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    else
    {
        Console.WriteLine("That was a not a number.");
    }
}
