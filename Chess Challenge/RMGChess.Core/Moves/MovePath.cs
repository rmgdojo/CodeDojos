using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public class MovePath
    {
        private IEnumerable<Position> _steps;

        public Position From { get; init; }
        public Position To { get; init; }
        public IEnumerable<Position> Steps => _steps ?? GetSteps();
        public Direction Direction { get; private set; }

        public override string ToString()
        {
            string output = "";
            foreach (Position step in Steps)
            {
                output += $"{step} -> ";
            }
            return output[..^4];
        }

        private IEnumerable<Position> GetSteps()
        {
            List<Position> stepList = new();
            int steps = 1;
            Position current = From;
            Position next = null;
            int fileDifference = To.File - From.File;
            int rankDifference = To.Rank - From.Rank;

            stepList.Add(current);

            if (Direction == Direction.LShaped)
            {
                // special case for knight moves
                // two variants exist: two vertical and one horizontal (tvoh), or two horizontal and one vertical (thov)
                // each can be left or right handed (relative to board files) and up or down (relative to the board ranks) 
                Dictionary<string, Dictionary<string, List<(int fileChange, int rankChange)>>> moves = new()
                {
                    { "tvoh", new Dictionary<string, List<(int, int)>>()
                        {
                            { "left down", new List<(int, int)> { (0, -1), (0, -1), (-1, 0) } },
                            { "left up", new List<(int, int)> { (0, 1), (0, 1), (-1, 0) } },
                            { "right down", new List<(int, int)> { (0, -1), (0, -1), (1, 0) } },
                            { "right up", new List<(int, int)> { (0, 1), (0, 1), (1, 0) } }
                        } 
                    },
                    { "thov", new Dictionary<string, List<(int, int)>>()
                        {
                            { "left down", new List<(int, int)> { (-1, 0), (0, -1), (0, -1) } },
                            { "left up", new List<(int, int)> { (-1, 0), (0, 1), (0, 1) } },
                            { "right down", new List<(int, int)> { (1, 0), (0, -1), (0, -1) } },
                            { "right up", new List<(int, int)> { (1, 0), (0, 1), (0, 1) } }
                        }
                    }
                };

                // determine the direction of the knight move
                string moveType = Math.Abs(fileDifference) == 2 ? "thov" : "tvoh";
                string direction = (fileDifference < 0 ? "left" : "right") + " " + (rankDifference < 0 ? "down" : "up");

                foreach(var step in moves[moveType][direction])
                {
                    next = new Position((char)(current.File + step.fileChange), current.Rank + step.rankChange);
                    stepList.Add(next);
                    current = next;
                }
            }
            else
            {
                // create a route consisting of all positions between From and To based on Direction
                do
                {
                    if (Direction != Direction.LShaped)
                    {
                        next = Direction switch
                        {
                            Direction.Up => new Position(current.File, current.Rank + 1),
                            Direction.Down => new Position(current.File, current.Rank - 1),
                            Direction.Left => new Position((char)(current.File - 1), current.Rank),
                            Direction.Right => new Position((char)(current.File + 1), current.Rank),
                            Direction.UpRight => new Position((char)(current.File + 1), current.Rank + 1),
                            Direction.UpLeft => new Position((char)(current.File - 1), current.Rank + 1),
                            Direction.DownRight => new Position((char)(current.File + 1), current.Rank - 1),
                            Direction.DownLeft => new Position((char)(current.File - 1), current.Rank - 1),
                            _ => null
                        };
                    }

                    stepList.Add(next);
                    steps++;
                    current = next;
                    if (next.Equals(To))
                    {
                        break; // reached the destination
                    }
                }
                while (true);
            }

            _steps = stepList; // so this is never called again
            return stepList;
        }

        public MovePath(Position from, Position to, Direction direction)
        {
            From = from;
            To = to;
            Direction = direction;
        }
    }
}
