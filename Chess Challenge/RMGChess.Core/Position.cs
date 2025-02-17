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
            File = Char.ToLower(file);
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{File}{Rank}";
        }

        public override bool Equals(object obj)
        {
            var position = obj as Position;
            if (position == null)
            {
                return false;
            }
            return position.File == this.File && position.Rank == this.Rank;
        }

        // implicit conversion from string to Position
        public static implicit operator Position(string position)
        {
            return new Position(position[0], position[1] - '0');
        }

        public static bool operator ==(Position a, string b)
        {
            return a.ToString() == b;
        }

        public static bool operator !=(Position a, string b)
        {
            return a.ToString() != b;
        }

        public static bool operator ==(string a, Position b)
        {
            return a == b.ToString();
        }

        public static bool operator !=(string a, Position b)
        {
            return a != b.ToString();
        }
    }
}
