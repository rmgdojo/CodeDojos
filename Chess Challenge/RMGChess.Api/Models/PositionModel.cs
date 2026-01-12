using RMGChess.Core;

namespace RMGChess.Api.Models;

/// <summary>
/// Represents a position on the chess board (e.g., "e4", "a1")
/// </summary>
public class PositionModel
{
    /// <summary>
    /// The file (column) of the position ('a' through 'h')
    /// </summary>
    public char File { get; set; }

    /// <summary>
    /// The rank (row) of the position (1 through 8)
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    /// String representation in algebraic notation (e.g., "e4")
    /// </summary>
    public string Notation => $"{char.ToLower(File)}{Rank}";

    public PositionModel() { }

    public PositionModel(char file, int rank)
    {
        File = file;
        Rank = rank;
    }

    public static PositionModel FromPosition(Position position)
    {
        if (position == null) return null;
        return new PositionModel(position.File, position.Rank);
    }
}
