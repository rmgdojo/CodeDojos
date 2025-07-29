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
            if (obj is null || obj is not Position) return false;
            return obj as Position == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Position(char file, int rank)
        {
            if (file < 'a' || file > 'h' || rank < 1 || rank > 8)
            {
                throw new InvalidPositionException($"Specified position '{file}{rank}' is invalid.");
            }

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

        // override ==
        public static bool operator ==(Position position, string positionAsString)
        {
            if (position is null || positionAsString is null)
                return position is null && positionAsString is null;

            return position.ToString().ToLower() == positionAsString.ToLower();
        }

        public static bool operator !=(Position position, string positionAsString)
        {
            return !(position == positionAsString);
        }

        public static bool operator ==(Position position, Position other)
        {
            if (position is null)
            {
                return other is null;
            }

            return position.File == other.File && position.Rank == other.Rank;
        }

        public static bool operator !=(Position position, Position other)
        {
            return !(position == other);
        }
    }
}
