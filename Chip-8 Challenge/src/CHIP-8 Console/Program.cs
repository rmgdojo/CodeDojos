using CHIP_8_Virtual_Machine;

namespace CHIP_8_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Tribble tribble = new Tribble(0x345);
            int i = tribble;
            ushort u = tribble;

            Tribble t2 = tribble + 1;
            int i2 = t2;
            ushort u2 = t2;
        }
    }
}
