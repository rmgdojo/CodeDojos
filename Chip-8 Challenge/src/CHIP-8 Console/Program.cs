using CHIP_8_Virtual_Machine;

namespace CHIP_8_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Tribble tribble = new Tribble(0x1345);
            
            
            VM vm = new VM();
            vm.Load(new byte[] { 0x31, 0xEE });
            //vm.Load("pong.rom");
            vm.Run();
        }
    }
}
