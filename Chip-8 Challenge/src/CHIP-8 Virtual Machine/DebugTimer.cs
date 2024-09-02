namespace CHIP_8_Virtual_Machine
{
    public class DebugTimer : ITimer
    {
        private int _cyclesSoFar;
        private int _targetCycles;
        private bool _started;

        public byte GetCyclesRemaining()
        {
            return (byte)(_targetCycles - _cyclesSoFar);
        }

        public void Start(int cycles)
        {
            if (!_started)
            {
                _targetCycles = cycles;
                _cyclesSoFar = 0;
                _started = true;
            }
        }

        private void OnAfterExecution(object sender, ExecutionResult e)
        {
            _cyclesSoFar++;
            if (_cyclesSoFar == _targetCycles)
            {
                _started = false;
            }
        }

        public DebugTimer(VM vm)
        {
            vm.OnAfterExecution += OnAfterExecution;
        }
    }
}