namespace CHIP_8_Virtual_Machine
{
    public interface IDebugVM
    {
        void Pause();
        void Resume();
        void ReplaceTimers(ITimer delayTimer, ITimer soundTimer);
    }
}