using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    public class Philosopher
    {
        private const int MINIMUM_TIME = 1000;
        private const int MAXIMUM_TIME = 10000;
       
        private Random _random = new Random(new Random(DateTime.Now.Millisecond).Next()); // doubly random!
        private Table _table;
        private ConsoleColor _color;
        
        public string Name { get; init; }
        public PhilosopherState State { get; private set; }

        public void Think()
        {
            Table.Log($"{Name} is thinking.", _color);
            State = PhilosopherState.Thinking;
            Thread.Sleep(_random.Next(MINIMUM_TIME, MAXIMUM_TIME));
        }

        public void Eat()
        {
            Chopsticks chopsticks;

            Table.Log($"{Name} wants to eat.", _color);
            while (!_table.TryGetChopsticks(out chopsticks)) Thread.Sleep(0);
            State = PhilosopherState.Eating;
            Table.Log($"{Name} acquired Chopsticks {chopsticks.Number}.", _color);
            Table.Log($"{Name} is eating.", _color);
            Thread.Sleep(_random.Next(MINIMUM_TIME, MAXIMUM_TIME));

            _table.ReturnChopsticksToTable(chopsticks);
        }

        public Philosopher(string name, Table table, ConsoleColor color)
        {
            Name = name;
            _table = table;
            _color = color;
        }
    }

    public enum PhilosopherState
    {
        Thinking, Eating
    }
}
