namespace RMGChess.Core
{
    [Flags]
    public enum MoveType
    {
        Vertical,
        Horizontal,
        Diagonal,
        LShaped,
        Castling,
        EnPassant,
        Promotion,
        NotBackwards,
        TakesDiagonally,
        None
    }
}
