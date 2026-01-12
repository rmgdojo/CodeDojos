using RMGChess.Core;

namespace RMGChess.Api.Models;

/// <summary>
/// Represents the complete state of a chess game
/// </summary>
public class GameStateModel
{
    /// <summary>
    /// Unique identifier for the game instance
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The color of the player whose turn it is to move
    /// </summary>
    public string ColourPlaying { get; set; }

    /// <summary>
    /// All pieces currently in play on the board
    /// </summary>
    public List<PieceModel> PiecesInPlay { get; set; }

    /// <summary>
    /// All pieces that have been captured
    /// </summary>
    public List<PieceModel> CapturedPieces { get; set; }

    /// <summary>
    /// The last move made in the game
    /// </summary>
    public MoveModel LastMove { get; set; }

    /// <summary>
    /// Move history for White player
    /// </summary>
    public List<MoveModel> WhiteMoveHistory { get; set; }

    /// <summary>
    /// Move history for Black player
    /// </summary>
    public List<MoveModel> BlackMoveHistory { get; set; }

    /// <summary>
    /// Whether White is currently in check
    /// </summary>
    public bool IsWhiteInCheck { get; set; }

    /// <summary>
    /// Whether Black is currently in check
    /// </summary>
    public bool IsBlackInCheck { get; set; }

    /// <summary>
    /// Whether the game has ended
    /// </summary>
    public bool IsGameEnded { get; set; }

    /// <summary>
    /// The reason the game ended (if applicable)
    /// </summary>
    public string GameEndReason { get; set; }

    /// <summary>
    /// Total number of moves made in the game
    /// </summary>
    public int TotalMoves { get; set; }

    /// <summary>
    /// Current round number
    /// </summary>
    public int CurrentRound { get; set; }

    public static GameStateModel FromGame(Game game)
    {
        if (game == null) return null;

        var whiteHistory = new List<MoveModel>();
        var blackHistory = new List<MoveModel>();

        // Build move histories
        int whiteCount = 0;
        int blackCount = 0;
        
        // Count moves to determine how many to retrieve
        try
        {
            while (true)
            {
                var move = game.HistoryFor(Colour.White, whiteCount);
                if (move == null) break;
                whiteHistory.Add(MoveModel.FromMove(move));
                whiteCount++;
            }
        }
        catch { /* End of history */ }

        try
        {
            while (true)
            {
                var move = game.HistoryFor(Colour.Black, blackCount);
                if (move == null) break;
                blackHistory.Add(MoveModel.FromMove(move));
                blackCount++;
            }
        }
        catch { /* End of history */ }

        return new GameStateModel
        {
            Id = game.Id, // Use the actual Game ID instead of generating a new one
            ColourPlaying = game.ColourPlaying.ToString(),
            PiecesInPlay = game.PiecesInPlay.Select(PieceModel.FromPiece).ToList(),
            CapturedPieces = game.CapturedPieces.Select(PieceModel.FromPiece).ToList(),
            LastMove = MoveModel.FromMove(game.LastMove),
            WhiteMoveHistory = whiteHistory,
            BlackMoveHistory = blackHistory,
            IsWhiteInCheck = game.IsInCheck(Colour.White),
            IsBlackInCheck = game.IsInCheck(Colour.Black),
            IsGameEnded = false, // This would need to be tracked separately in the game
            GameEndReason = null,
            TotalMoves = whiteHistory.Count + blackHistory.Count,
            CurrentRound = Math.Max(whiteHistory.Count, blackHistory.Count)
        };
    }
}
