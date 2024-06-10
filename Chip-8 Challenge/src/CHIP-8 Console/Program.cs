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
                    if (keypad.IsDown(0x01))
                    {
                        Console.WriteLine("Key is down!");
                    }

                    keypad.KeyUp(key.KeyChar.ToString());
                    if (!keypad.IsDown(0x01))
                    {
                        Console.WriteLine("Key is up!");
                    }
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Key is not mapped.");
                }
            }
        }
    }
}
