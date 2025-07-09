namespace RMGChess.Core;

public class GameRecord
{
    public string Name => $"{Event} | {Date.ToShortDateString()} | {PlayingWhite} vs {PlayingBlack}";
    public string Event { get; init; }
    public DateTime Date { get; init; }
    public string PlayingWhite { get; init; }
    public string PlayingBlack { get; init; }
    public RoundRecord[] Rounds { get; init; }
    public int RoundCount => Rounds.Length;

    public GameRecord(string @event, DateTime date, string playingWhite, string playingBlack, string[] movesAsAlgebra)
    {
        Event = @event;
        Date = date;
        PlayingWhite = playingWhite;
        PlayingBlack = playingBlack;

        List<RoundRecord> rounds = new();
        for (int i = 0; i < movesAsAlgebra.Length; i += 2)
        {
            string whiteMoveAsAlgebra = movesAsAlgebra[i];
            MoveRecord whiteMove = new MoveRecord(whiteMoveAsAlgebra, Colour.White);
            MoveRecord blackMove = null;
            if (i + 1 < movesAsAlgebra.Length)
            {
                string blackMoveAsAlgebra = movesAsAlgebra[i + 1];
                blackMove = new MoveRecord(blackMoveAsAlgebra, Colour.Black);
            }
            rounds.Add(new RoundRecord((i / 2) + 1, whiteMove, blackMove));
        }

        Rounds = rounds.ToArray();
    }
}

public class RoundRecord
{
    public int RoundIndex { get; init; }
    public MoveRecord WhiteMove { get; init; }
    public MoveRecord BlackMove { get; init; }
    public MoveRecord[] Moves => new[] { WhiteMove, BlackMove };

    public RoundRecord(int roundIndex, MoveRecord whiteMove, MoveRecord blackMove)
    {
        RoundIndex = roundIndex;
        WhiteMove = whiteMove;
        BlackMove = blackMove;
    }
}

public class MoveRecord
{
    public string MoveAsAlgebra { get; init; }
    public Colour WhoseTurn { get; init; }

    public Move GetMove(Game game)
    {
        return Algebra.DecodeAlgebra(MoveAsAlgebra, game.Board, WhoseTurn);
    }

    public MoveRecord(string moveAsAlgebra, Colour whoseTurn)
    {
        MoveAsAlgebra = moveAsAlgebra;
        WhoseTurn = whoseTurn;
    }
}