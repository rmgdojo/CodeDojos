namespace RMGChess.Core;

public class GameRecord
{
    public string Name => $"{Event} | {Date.ToShortDateString()} | {PlayingWhite} vs {PlayingBlack}";
    public string Event { get; init; }
    public DateTime Date { get; init; }
    public string PlayingWhite { get; init; }
    public string PlayingBlack { get; init; }
    public string[] MovesAsAlgebra { get; init; }

    public GameRecord(string @event, DateTime date, string playingWhite, string playingBlack, string[] movesAsAlgebra)
    {
        Event = @event;
        Date = date;
        PlayingWhite = playingWhite;
        PlayingBlack = playingBlack;
        MovesAsAlgebra = movesAsAlgebra;
    }
}