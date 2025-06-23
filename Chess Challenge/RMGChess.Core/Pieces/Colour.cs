namespace RMGChess.Core
{
    public enum Colour
    {
        White,
        Black
    }

    public static class ColourExtensions
    {
        public static Colour Switch(this Colour colour)
        {
            return colour == Colour.White ? Colour.Black : Colour.White;
        }
    }
}
