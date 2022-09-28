using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Grid
    {
        private Cell[,] _cells;
        private Random _random = new Random(DateTime.Now.Millisecond);

        public int Width { get; init; }
        public int Height { get; init; }

        public Cell this[int x, int y] => (x > 0 && x < (Width-1)) && (y > 0 && y < (Height-1)) ? _cells[x, y] : new Cell(x, y, false);

        public void Initialise(bool[,] cellMap)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _cells[j, i] = new Cell(j, i, cellMap[j, i]);
                }
            }
        }

        public void InitialiseRandom()
        {
            int cells = Width * Height;
            int set = 0;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    bool alive = _random.Next(0, 10) <= 5;
                    if (alive) set++;
                    if (set > (cells / 2)) alive = false;

                    _cells[j, i] = new Cell(j, i, alive);
                }
            }
        }

        public void Tick()
        {
            Grid now = new Grid(this);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _cells[j, i].Tick(now);
                }
            }
        }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new Cell[Width, Height];
        }

        public Grid(Grid original)
        {
            Width = original.Width;
            Height = original.Height;

            _cells = new Cell[Width, Height];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _cells[j, i] = new Cell(j, i, original[j, i].Alive);
                }
            }
        }

        public override string ToString()
        {
            string map = String.Empty;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    map += _cells[j, i].Alive ? "*" : ".";
                }
                map += "\r\n";
            }

            return map;
        }
    }
}
