using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    public class Chopsticks
    {
        private bool _inUse;
        
        public int Number { get; init; }

        public bool InUse 
        {
            get { return _inUse; }
            set
            {
                _inUse = value;
                Table.Log($"Chopsticks {Number} is {(_inUse ? "in use" : "available")}.", ConsoleColor.Yellow);
            }
        }

        public Chopsticks(int number)
        {
            Number = number;
        }
    }
}
