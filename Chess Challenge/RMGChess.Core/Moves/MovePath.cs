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
                output += $"{step}->";
            }
            return output[..^2];
        }

        private IEnumerable<Position> GetSteps()
        {
            List<Position> stepList = new();
            Position current = From;
            Position next = null;
            int fileDifference = To.File - From.File;
            int rankDifference = To.Rank - From.Rank;

            stepList.Add(current);

            if (Direction == Direction.LShaped)
            {
                // same idea as before, but simpler implementation #GHCP

                // this is a lookup table for knight moves keyed by the file and rank difference
                Dictionary<(int, int), (int fileChange, int rankChange)[]> knightMoves = new()
                {
                    [(2, 1)] = [(1, 0), (1, 0), (0, 1)],     // right up
                    [(2, -1)] = [(1, 0), (1, 0), (0, -1)],   // right down
                    [(-2, 1)] = [(-1, 0), (-1, 0), (0, 1)],  // left up
                    [(-2, -1)] = [(-1, 0), (-1, 0), (0, -1)],// left down
                    [(1, 2)] = [(0, 1), (0, 1), (1, 0)],     // up right
                    [(-1, 2)] = [(0, 1), (0, 1), (-1, 0)],   // up left
                    [(1, -2)] = [(0, -1), (0, -1), (1, 0)],  // down right
                    [(-1, -2)] = [(0, -1), (0, -1), (-1, 0)] // down left
                };

                foreach (var step in knightMoves[(fileDifference, rankDifference)])
                {
                    char newFile = (char)(current.File + step.fileChange);
                    int newRank = current.Rank + step.rankChange;

                    next = new Position(newFile, newRank); // will throw if invalid
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
                    current = next;
                    if (next == To)
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
