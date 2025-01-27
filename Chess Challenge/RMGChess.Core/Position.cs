namespace RMGChess.Core
{
    public class Position
    {
        public char File { get; }
        public int Rank { get; }

        public void Deconstruct(out char File, out int Rank)
        {
            File = this.File;
            Rank = this.Rank;
        }

        public Position(char file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{Char.ToLower(File)}{Rank}";
        }

        // implicit conversion from string to Position
        public static implicit operator Position(string position)
        {
            return new Position(position[0], position[1] - '0');
        }
    }
}
