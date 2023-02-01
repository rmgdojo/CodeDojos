namespace CircuitBreaker
{
    public class FailureThresholdExceededException : Exception
    {
        public FailureThresholdExceededException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}