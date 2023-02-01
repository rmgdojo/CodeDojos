namespace CircuitBreaker
{
    public class FirstCallAfterTimeoutFailedException : Exception
    {
        public FirstCallAfterTimeoutFailedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}