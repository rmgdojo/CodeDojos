namespace Throttler
{
    internal class Program
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);
        private static bool _go = true;

        static void Main(string[] args)
        {
            Throttle throttle = new Throttle(true);
            throttle.OnWindowClosed += Throttle_OnWindowClosed;
            throttle.OpenWindow(5000, 3);
            while (_go)
            {
                try
                {
                    if (!throttle.Accept(() => Console.WriteLine("Hello there!")))
                    {
                        Console.WriteLine("Queued for later execution.");
                    }
                }
                catch (ThrottleException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Thread.Sleep(_random.Next(100, 500));
                }
            }

            Console.WriteLine("\nWe're done!");
            Console.ReadLine();
        }

        private static void Throttle_OnWindowClosed(object? sender, EventArgs e)
        {
            //_go = false;
            ((Throttle)sender)?.OpenWindow(_random.Next(1000, 5000), _random.Next(1, 5));
        }
    }
}