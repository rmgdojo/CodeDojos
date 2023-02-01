using CircuitBreaker;

namespace CircuitBreakerTest
{
    internal class Program
    {
        private static Random _random = new Random();
        
        static void Main(string[] args)
        {
            Breaker<int> breaker = new Breaker<int>(TimeSpan.FromSeconds(5), 5, TestFunction);
            breaker.OnTimeoutBegun += Breaker_OnTimeoutBegun;
            breaker.OnTimeoutEnded += Breaker_OnTimeoutEnded;

            while (true)
            {
                Console.Write($"Failures: {breaker.FailuresSinceClosed} of {breaker.FailureThreshold}. ");
                Console.Write("Calling...");
                bool failed = !breaker.TryCall(0, out bool open, out TimeSpan timeoutRemaining);
                if (failed || open)
                {                    
                    Console.Write("Failed. ");
                    
                    if (open)
                    {
                        Console.Write($"Timeout remaining: {timeoutRemaining.Seconds} seconds.");
                    }
                    
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Succeeded.");
                }

                Thread.Sleep(1000);
            }
        }

        private static void Breaker_OnTimeoutBegun(object? sender, EventArgs e)
        {
            Console.Write("Failure threshold reached! Circuit breaker is now OPEN. ");
        }

        private static void Breaker_OnTimeoutEnded(object? sender, EventArgs e)
        {
            Console.WriteLine("Timeout expired. Circuit breaker is now HALF-OPEN.");
        }

        private static void TestFunction(int input)
        {
            bool succeeds = _random.Next(0, 100) % 2 == 0;
            if (!succeeds)
            {
                throw new InvalidOperationException();
            }
        }
    }
}