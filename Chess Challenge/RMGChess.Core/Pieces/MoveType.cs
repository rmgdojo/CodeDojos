namespace RMGChess.Core
{
    [Flags]
    public enum MoveType
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        Diagonal = 4,
        LShaped = 8,
        Castling = 16,
        EnPassant = 32,
        Promotion = 64,
        NotBackwards = 128,
        TakesDiagonally = 256
    }
}
