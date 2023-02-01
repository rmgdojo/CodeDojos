using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireHazard
{
    public enum StateChange
    {
        On, Off, Toggle
    }

    public class LightGrid
    {
        private int _rows;
        private int _columns;
        private bool[,] _lights;

        public int Lights => _lights.Cast<bool>().Count();
        public int LightsOn => _lights.Cast<bool>().Where(x => x).Count();
        public int LightsOff => _lights.Cast<bool>().Where(x => !x).Count();

        public bool IsOn(int row, int column)
        {
            return _lights[row, column];
        }

        public void TurnOn(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            Set(fromRow, fromColumn, toRow, toColumn, StateChange.On);
        }

        public void TurnOff(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            Set(fromRow, fromColumn, toRow, toColumn, StateChange.Off);
        }

        public void Toggle(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            Set(fromRow, fromColumn, toRow, toColumn, StateChange.Toggle);
        }

        private void Set(int fromRow, int fromColumn, int toRow, int toColumn, StateChange change)
        {
            if (fromRow < 0 || fromColumn < 0 || toRow > (_rows -1) || toColumn > (_columns -1))
            {
                throw new ArgumentOutOfRangeException("Specified coordinate was off the grid.");
            }

            for (int r = fromRow; r <= toRow; r++)
            {
                for (int c = fromColumn; c <= toColumn; c++)
                {
                    _lights[r, c] = change switch
                    {
                        StateChange.On => true,
                        StateChange.Off => false,
                        StateChange.Toggle => !_lights[r, c],
                        _ => _lights[r, c]
                    };
                }
            }
        }

        public override string ToString()
        {
            string output = "";
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    output += _lights[r, c] ? "*" : ".";
                }
                output += "\r\n";
            }

            return output;
        }

        public LightGrid(int rows, int columns) 
        { 
            _rows = rows;
            _columns = columns;
            _lights = new bool[rows, columns];
        }
    }
}
