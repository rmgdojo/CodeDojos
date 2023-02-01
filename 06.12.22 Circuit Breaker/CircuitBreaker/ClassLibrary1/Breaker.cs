using System.Threading.Tasks.Dataflow;

namespace CircuitBreaker
{
    public class Breaker<T>
    {
        private Action<T> _function;
        private bool _open;
        private bool _halfOpen;
        private int _failures;

        private Timer? _timer;
        private DateTime? _timerStarted;

        public TimeSpan TimeOut { get; init; }
        public int FailureThreshold { get; init; }
        public int FailuresSinceClosed => _failures;
        public bool Open => _open;
        public bool HalfOpen => _halfOpen;

        public event EventHandler OnTimeoutBegun;
        public event EventHandler OnTimeoutEnded;

        public void Call(T parameter)
        {
            if (!_open)
            {
                try
                {
                    _function(parameter);
                    if (_halfOpen)
                    {
                        _halfOpen = false;
                        _failures = 0;
                    }
                }
                catch (Exception ex)
                {
                    // failed
                    _failures++;
                    if (_halfOpen)
                    {
                        _failures = 1;
                        _open = true;
                        _halfOpen = false;
                        BeginTimeout();
                        throw new FirstCallAfterTimeoutFailedException($"The first call after timeout failed and the circuit breaker has been re-opened.", ex);
                    } 
                    else if (_failures == FailureThreshold)
                    {
                        _open = true;
                        _halfOpen = false;
                        BeginTimeout();
                        throw new FailureThresholdExceededException($"{_failures} calls have failed and the circuit breaker is now open.", ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                TimeSpan remaining = TimeOut - (DateTime.Now - (_timerStarted ?? DateTime.Now));
                throw new CircuitOpenException($"The circuit breaker is open and all calls will fail. Time remaining to breaker reset: {remaining.Seconds } seconds", remaining);
            }
        }

        public bool TryCall(T parameter, out bool circuitOpen, out TimeSpan timeoutRemaining)
        {
            circuitOpen = false;
            timeoutRemaining = TimeSpan.Zero;

            try
            {
                Call(parameter);
            }
            catch (Exception ex)
            {
                if (ex is CircuitOpenException)
                {
                    circuitOpen = true;
                    timeoutRemaining = ((CircuitOpenException)ex).TimeoutRemaining;
                }

                return false;
            }

            return true;
        }

        private void BeginTimeout()
        {
            _timer = new Timer(TimeoutCompleted, null, TimeOut, Timeout.InfiniteTimeSpan);
            _timerStarted = DateTime.Now;
            OnTimeoutBegun?.Invoke(this, EventArgs.Empty);
        }

        private void TimeoutCompleted(object? state)
        {
            _timer = null;
            _timerStarted = null;
            _open = false;
            _halfOpen = true;
            OnTimeoutEnded?.Invoke(this, EventArgs.Empty);
        }

        public Breaker(TimeSpan timeout, int failureThreshold, Action<T> function)
        {
            TimeOut = timeout; 
            FailureThreshold = failureThreshold;
            _function = function;
        }
    }
}