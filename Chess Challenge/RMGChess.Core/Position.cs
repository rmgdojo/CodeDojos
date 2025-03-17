namespace RMGChess.Core
{
    public class Position
    {
        public char File { get; }
        public int Rank { get; }

        public Side Side => File switch { < 'e' => Side.Queenside, >= 'e' => Side.Kingside };

        public void Deconstruct(out char File, out int Rank)
        {
            File = this.File;
            Rank = this.Rank;
        }

        public override string ToString()
        {
            return $"{Char.ToLower(File)}{Rank}";
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Position(char file, int rank)
        {
            File = file;
            Rank = rank;
        }

        // implicit conversion from string to Position
        public static implicit operator Position(string position)
        {
            if (position.Length != 2 || position[0] < 'a' || position[0] > 'h' || position[1] < '1' || position[1] > '8')
            {
                throw new InvalidPositionException($"Specfied position '{position}' is invalid.");
            }

            return new Position(position[0], position[1] - '0');
        }

        public static implicit operator Position((char file, int rank) position)
        {
            if (position.file < 'a' || position.file > 'h' || position.rank < 1 || position.rank > 8)
            {
                throw new InvalidPositionException($"Specfied position '{position.file}{position.rank}' is invalid.");
            }
            return new Position(position.file, position.rank);
        }
    }
}
