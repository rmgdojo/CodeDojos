using CHIP_8_Virtual_Machine;

namespace CHIP_8_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Keypad keypad = new();
            while (true)
            {
                try
                {
                    var key = Console.ReadKey(false);

                    keypad.KeyDown(key.KeyChar.ToString());

                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Key is not mapped.");
                }
            }
        }
    }
}
