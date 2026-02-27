namespace RMGChess.Api.Models;

/// <summary>
/// Request body for making a move in a live game
/// </summary>
public class MakeMoveRequest
{
    /// <summary>
    /// The source square in algebraic notation (e.g., "e2")
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// The destination square in algebraic notation (e.g., "e4")
    /// </summary>
    public string To { get; set; }
}
