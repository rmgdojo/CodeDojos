using CHIP_8_Virtual_Machine;

namespace CHIP_8_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            VM vm = new VM();
            vm.Load(new byte[] { 0x0F, 0xFF });
            //vm.Load("pong.rom");
            vm.Run();
        }
    }
}
