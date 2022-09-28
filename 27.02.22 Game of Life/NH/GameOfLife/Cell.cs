using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Cell
    {
        private int _x;
        private int _y;

        public bool Alive { get; private set; }

        public void Tick(Grid grid)
        {
            Cell[] neighbours = new Cell[] { 
                grid[_x-1, _y-1],
                grid[_x-1, _y],
                grid[_x-1, _y+1],
                grid[_x, _y-1],
                grid[_x, _y+1],
                grid[_x+1, _y-1],
                grid[_x+1, _y],
                grid[_x+1, _y+1],
            };

            int neighboursAlive = neighbours.Count(x => x.Alive);

            if (Alive)
            {
                if (neighboursAlive < 2 || neighboursAlive > 3)
                {
                    Alive = false;
                }
            }
            else
            {
                if (neighboursAlive == 3)
                {
                    Alive = true;
                }
            }
        }

        public Cell(int x, int y, bool alive)
        {
            _x = x;
            _y = y;
            Alive = alive;
        }
    }
}
