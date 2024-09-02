namespace CHIP_8_Virtual_Machine
{
    public interface ITimer
    {
        byte GetCyclesRemaining();
        void Start(int cycles);
    }
}