using RMGChess.Core;

namespace RMGChess.Api.Models;

/// <summary>
/// Represents a chess piece in the game
/// </summary>
public class PieceModel
{
    /// <summary>
    /// The color of the piece ("White" or "Black")
    /// </summary>
    public string Colour { get; set; }

    /// <summary>
    /// The type of piece (e.g., "King", "Queen", "Rook", "Bishop", "Knight", "Pawn")
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The piece's symbol (e.g., 'K', 'Q', 'R', 'B', 'N', 'P')
    /// </summary>
    public char Symbol { get; set; }

    /// <summary>
    /// Current position of the piece on the board
    /// </summary>
    public PositionModel Position { get; set; }

    /// <summary>
    /// Original starting position of the piece
    /// </summary>
    public PositionModel Origin { get; set; }

    /// <summary>
    /// Whether the piece has moved from its starting position
    /// </summary>
    public bool HasMoved { get; set; }

    /// <summary>
    /// The point value of the piece
    /// </summary>
    public int Value { get; set; }

    public static PieceModel FromPiece(Piece piece)
    {
        if (piece == null) return null;

        return new PieceModel
        {
            Colour = piece.Colour.ToString(),
            Type = piece.GetType().Name,
            Symbol = piece.Symbol,
            Position = PositionModel.FromPosition(piece.Position),
            Origin = PositionModel.FromPosition(piece.Origin),
            HasMoved = piece.HasMoved,
            Value = piece.Value
        };
    }
}
