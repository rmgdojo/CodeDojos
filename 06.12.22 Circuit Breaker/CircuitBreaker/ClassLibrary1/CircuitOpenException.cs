namespace CircuitBreaker
{
    public class CircuitOpenException : Exception
    {
        private TimeSpan _timeoutRemaining;

        public TimeSpan TimeoutRemaining => _timeoutRemaining;

        public CircuitOpenException(string message, TimeSpan timeoutRemaining)
            : base(message) 
        { 
            _timeoutRemaining= timeoutRemaining;
        }
    }
}