using RMGChess.Core;

namespace RMGChess.Api.Models;

/// <summary>
/// Represents a chess move in the game
/// </summary>
public class MoveModel
{
    /// <summary>
    /// The piece being moved
    /// </summary>
    public PieceModel Piece { get; set; }

    /// <summary>
    /// Starting position of the move
    /// </summary>
    public PositionModel From { get; set; }

    /// <summary>
    /// Destination position of the move
    /// </summary>
    public PositionModel To { get; set; }

    /// <summary>
    /// The color of the player making the move
    /// </summary>
    public string WhoIsMoving { get; set; }

    /// <summary>
    /// Whether this move captures an opponent's piece
    /// </summary>
    public bool TakesPiece { get; set; }

    /// <summary>
    /// The piece being captured (if any)
    /// </summary>
    public PieceModel PieceToTake { get; set; }

    /// <summary>
    /// Whether this move puts the opponent in check
    /// </summary>
    public bool PutsOpponentInCheck { get; set; }

    /// <summary>
    /// Whether this is a pawn promotion move
    /// </summary>
    public bool IsPromotion { get; set; }

    /// <summary>
    /// The piece type to promote to (e.g., "Queen")
    /// </summary>
    public string PromotesTo { get; set; }

    /// <summary>
    /// String representation of the move
    /// </summary>
    public string MoveNotation { get; set; }

    public static MoveModel FromMove(Move move)
    {
        if (move == null) return null;

        return new MoveModel
        {
            Piece = PieceModel.FromPiece(move.Piece),
            From = PositionModel.FromPosition(move.From),
            To = PositionModel.FromPosition(move.To),
            WhoIsMoving = move.WhoIsMoving.ToString(),
            TakesPiece = move.TakesPiece,
            PieceToTake = PieceModel.FromPiece(move.PieceToTake),
            PutsOpponentInCheck = move.PutsOpponentInCheck,
            IsPromotion = move.IsPromotion,
            PromotesTo = move.PromotesTo,
            MoveNotation = move.ToString()
        };
    }
}
