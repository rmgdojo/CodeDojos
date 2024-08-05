using Timer = System.Timers.Timer;

namespace CHIP_8_Virtual_Machine
{
    public class Timer
    {
        private const int STANDARD_INTERVAL = 17;
        private const int FAST_INTERVAL = 16;
        private const int FAST_THRESHOLD = 40;
        private const int STANDARD_THRESHOLD = 60;

        private System.Timers.Timer _timer;
        private int _cyclesSoFar;
        private int _targetCycles;
        
        public void Start(int cycles)
        {
            _timer.Start();
        }

        private void HandleIntervalChange(object sender, EventArgs e)
        {
            _cyclesSoFar++;
            _timer.Interval = _cyclesSoFar switch
            {
                >= STANDARD_THRESHOLD => STANDARD_INTERVAL,
                >= FAST_THRESHOLD => FAST_INTERVAL,
                _ => STANDARD_INTERVAL
            };

            if (_cyclesSoFar == _targetCycles)
            {
                _timer.Stop();
                // do something, not quite sure what yet
            }
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            // do something on each tick of the timer (do we need this?)
        }

        public Timer()
        {
            _timer = new System.Timers.Timer(STANDARD_INTERVAL);
            _timer.Elapsed += TimerElapsed;
            _timer.Elapsed += HandleIntervalChange;
        }
    }
}