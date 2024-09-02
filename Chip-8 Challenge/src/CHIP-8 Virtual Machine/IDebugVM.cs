namespace CHIP_8_Virtual_Machine
{
    public interface IDebugVM
    {
        void ReplaceTimers(ITimer delayTimer, ITimer soundTimer);
    }
}