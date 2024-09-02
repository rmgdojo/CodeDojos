namespace CHIP_8_Virtual_Machine
{
    public class Timer : ITimer
    {
        private const int STANDARD_INTERVAL = 17;
        private const int FAST_INTERVAL = 16;
        private const int FAST_THRESHOLD = 40;
        private const int STANDARD_THRESHOLD = 60;

        private System.Timers.Timer _windowsTimer;
        private int _cyclesSoFar;
        private int _targetCycles;
        
        public void Start(int cycles)
        {
            _targetCycles = cycles;
            _cyclesSoFar = 0;
            _windowsTimer.Start();
        }

        public byte GetCyclesRemaining()
        {
           return (byte)(_targetCycles - _cyclesSoFar);
        }

        private void HandleIntervalChange(object sender, EventArgs e)
        {
            // the timer runs at 60Hz, which means 60 timer cycles = 1 second = 1000 milliseconds
            // you cannot divide 1000 by 60 evenly, so we need to adjust the interval to account for the drift
            // we use an interval of 17ms (STANDARD_INTERVAL) for the first 40 cycles (FAST_THRESHOLD)
            // and then switch to an interval of 16ms (FAST_INTERVAL) for the remaining 20 cycles
            // until we reach 60 cycles (STANDARD_THRESHOLD) and then we switch back to STANDARD_INTERVAL
            // note that if the number of cycles required for the timer is less than 60, some drift will still occur

            _cyclesSoFar++;

            // change the timer interval based on the number of cycles we have completed
            _windowsTimer.Interval = _cyclesSoFar switch
            {
                >= STANDARD_THRESHOLD => STANDARD_INTERVAL,
                >= FAST_THRESHOLD => FAST_INTERVAL,
                _ => STANDARD_INTERVAL
            };

            // when the required number of cycles has been reached, we stop the timer
            // and reset the cycle count
            if (_cyclesSoFar == _targetCycles)
            {
                _windowsTimer.Stop();
                // do something, not quite sure what yet?
            }
        }

        private void WindowsTimerElapsed(object sender, EventArgs e)
        {
            // do something on each tick of the timer (do we need this?)
        }

        public Timer()
        {
            _windowsTimer = new System.Timers.Timer(STANDARD_INTERVAL);
            _windowsTimer.Elapsed += WindowsTimerElapsed;
            _windowsTimer.Elapsed += HandleIntervalChange;
        }
    }
}