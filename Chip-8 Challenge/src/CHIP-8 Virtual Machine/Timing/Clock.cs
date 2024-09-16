using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace CHIP_8_Virtual_Machine
{
    public class Clock
    {
        private Timer _timer;
        private Stopwatch _stopwatch;
        private Thread _thread;
        private Action _callback;
        private bool _running;
        private bool _paused;
        private int _cycleTime;

        public ClockMode Mode { get; init; }

        public void Start()
        {
            _running = true;
            _timer?.Start();
            _thread?.Start();
        }

        public void Stop()
        {
            _running = false;
            _timer?.Stop();
            _thread?.Join();
        }

        public void Pause()
        {
            _paused = true;
            _timer?.Stop();
        }

        public void Resume()
        {
            _paused = false;
            _timer?.Start();
        }

        private void MainThread()
        {
            while (_running)
            {
                if (!_paused)
                {
                    if (_cycleTime > 0)
                    {
                        _stopwatch.Start();
                        _callback();
                        while (_stopwatch.ElapsedMilliseconds < _cycleTime) ;
                        _stopwatch.Reset();
                    }
                    else
                    {
                        _callback();
                    }
                }
            }
        }

        public Clock(ClockMode mode, Action callback, int cycleTimeInMilliseconds)
        {
            Mode = mode;
            _callback = callback;
            _cycleTime = cycleTimeInMilliseconds;
            _stopwatch = new Stopwatch();

            if (mode == ClockMode.Timer)
            {
                _timer = new Timer();
                _timer.Interval = cycleTimeInMilliseconds;
                _timer.Elapsed += (sender, e) => callback();
            }
            else
            {
                _thread = new Thread(MainThread);
            }
        }
    }
}
