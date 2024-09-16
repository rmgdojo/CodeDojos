namespace CHIP_8_Virtual_Machine
{
    public interface ITimer
    {
        event EventHandler<int> OnStart;
        event EventHandler OnElapsed;

        byte GetCyclesRemaining();
        void Start(int cycles);
    }
}