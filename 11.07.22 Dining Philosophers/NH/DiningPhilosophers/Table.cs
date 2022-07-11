using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    public class Table : IDisposable
    {
        public static object _lockObject = new object();
        public static void Log(string message, ConsoleColor color)
        {
            lock (_lockObject)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
            }
        }

        private bool _dining;

        private IDictionary<Thread, Philosopher> _threads = new Dictionary<Thread, Philosopher>();

        public IEnumerable<Chopsticks> Chopsticks { get; init; }
        public IEnumerable<Philosopher> Philosophers { get; init; }

        public bool TryGetChopsticks(out Chopsticks chopsticks)
        {
            foreach (Chopsticks c in Chopsticks)
            {
                lock (c)
                {
                    if (!c.InUse)
                    {
                        c.InUse = true;
                        chopsticks = c;
                        return true;
                    }
                }
            }

            chopsticks = null;
            return false;
        }

        public void ReturnChopsticksToTable(Chopsticks chopsticks)
        {
            lock (chopsticks)
            {
                chopsticks.InUse = false;
            }
        }

        public void BeginDinner()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            Console.WriteLine("Time for dinner!");
            Console.WriteLine($"There are {Philosophers.Count()} guests and {Chopsticks.Count()} pairs of chopsticks available.");
            Console.Write("Dinner begins in ");
            (int left, int top) = Console.GetCursorPosition();
            for (int i = 5; i > 0; i--)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(i);
                Thread.Sleep(1000);
            }
            Console.WriteLine("\n");

            _dining = true;
            foreach (Philosopher philosopher in Philosophers)
            {
                Thread thread = new Thread(Dine);
                _threads.Add(thread, philosopher);
                thread.Start();
            }
        }

        public void Dispose()
        {
            _dining = false;
        }

        private void Dine()
        {
            while (_dining)
            {
                Thread thread = Thread.CurrentThread;
                Philosopher philosopher = _threads[thread];

                philosopher.Think();
                philosopher.Eat();
            }
        }

        public Table(int numberOfPairsOfChopsticks, int numberOfPhilosophers)
            : this(numberOfPairsOfChopsticks, numberOfPhilosophers, new string[0])
        {
        }

        public Table(int numberOfPairsOfChopsticks, int numberOfPhilosophers, params string[] names)
        {
            if (numberOfPairsOfChopsticks < 1 || numberOfPhilosophers > 14) throw new ArgumentException("Must be at least 1 chopsticks and max 15 philosophers. Because I say so.");

            var chopsticks = new List<Chopsticks>();
            for (int i = 0; i < numberOfPairsOfChopsticks; i++)
            {
                chopsticks.Add(new Chopsticks(i+1));
            }

            var philosophers = new List<Philosopher>();
            for (int i = 0; i < numberOfPhilosophers; i++)
            {
                string name = (names.Length >= i) ? names[i] : $"Philosopher {i + 1}";
                philosophers.Add(new Philosopher(name, this, (ConsoleColor)i+1));
            }

            Chopsticks = chopsticks;
            Philosophers = philosophers;

            Console.CursorVisible = false;
            Console.Clear();
        }
    }
}
