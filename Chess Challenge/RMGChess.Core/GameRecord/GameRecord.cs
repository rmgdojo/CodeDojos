using RMGChess.Core;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;

namespace RMGChess.Core;

public delegate void BeforeMoveHandler(float roundIndex, Colour whoseTurn, string moveAsAlgebra, Move move, string lastMoveAsAlgebra, Move lastMove, TimeSpan decodeTime);
public delegate PlayControl AfterMoveHandler(float roundIndex, Colour whoseTurn, Move move);
public delegate bool ErrorHandler(string errorMessage, float roundIndex, Colour whoseTurn, Move lastMove, Move thisMove);

public class GameRecord
{
    private MoveRecord[] _moves;
    private RoundRecord[] _rounds;

    public string Name => $"{Event} | {Date.ToShortDateString()} | {PlayingWhite} vs {PlayingBlack}";
    public string Event { get; init; }
    public DateTime Date { get; init; }
    public string PlayingWhite { get; init; }
    public string PlayingBlack { get; init; }
    public IEnumerable<MoveRecord> Moves => _moves;
    public IEnumerable<RoundRecord> Rounds => _rounds;
    public int MoveCount => _moves.Length;
    public int RoundCount => _rounds.Length;

    public void Playback(Game game, BeforeMoveHandler beforeMove, AfterMoveHandler afterMove, ErrorHandler onError)
    {
        game.Reset();

        Move lastMove = null;
        string lastMoveAsAlgebra = null;

        for (int i = 1; i <= MoveCount; i++)
        {
            (PlayControl control, lastMove, lastMoveAsAlgebra) = PlayRecordedMove(game, _moves[i - 1], lastMove, lastMoveAsAlgebra, beforeMove, afterMove, onError);
            if (control.Stop)
            {
                return; // stop processing further moves
            }
            else if (control.GoToRound > 0)
            {
                RestartAndFastForwardRecordedGame(game, control.GoToRound, onError);
            }
        }
    }

    private void RestartAndFastForwardRecordedGame(Game game, float roundToFastForwardTo, ErrorHandler onError)
    {
        game.Reset();

        if (roundToFastForwardTo > 1)
        {
            int limit = (int)(roundToFastForwardTo * 2) - 2;
            for (int i = 0; i < limit; i++)
            {
                PlayRecordedMove(game, _moves[i], null, null, null, null, onError);
            }
        }
    }

    private (PlayControl control, Move move, string moveAsAlgebra) PlayRecordedMove(Game game, MoveRecord moveRecord, Move lastMove, string lastMoveAsAlgebra,
        BeforeMoveHandler beforeMove, AfterMoveHandler afterMove, ErrorHandler onError)
    {
        Move move = null;
        PlayControl control = null;

        string moveAsAlgebra = moveRecord.MoveAsAlgebra;
        Colour whoseTurn = moveRecord.WhoseTurn;
        float actualRoundIndex = moveRecord.RoundIndex;

        try
        {
            DateTime start = DateTime.Now;
            move = Algebra.DecodeAlgebra(moveAsAlgebra, game.Board, whoseTurn);
            TimeSpan decodeTime = DateTime.Now - start;

            beforeMove?.Invoke(actualRoundIndex, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove, decodeTime);
            game.MakeMove(move);

            control = afterMove?.Invoke(actualRoundIndex, whoseTurn, move) ?? new PlayControl();
        }
        catch (Exception ex)
        {
            if (onError?.Invoke($"Error in move '{moveAsAlgebra}': {ex.Message}", actualRoundIndex, whoseTurn, lastMove, move) ?? true)
            {
                return (new PlayControl(stop: true), move, moveAsAlgebra); // stop processing if an error occurs
            }
        }

        return (control, move, moveAsAlgebra);
    }

    public GameRecord(string @event, DateTime date, string playingWhite, string playingBlack, string[] movesAsAlgebra)
    {
        Event = @event;
        Date = date;
        PlayingWhite = playingWhite;
        PlayingBlack = playingBlack;

        List<MoveRecord> moveRecords = new List<MoveRecord>();
        bool white = true;
        for (int i = 0; i < movesAsAlgebra.Length; i++)
        {
            moveRecords.Add(new MoveRecord(i + 1, movesAsAlgebra[i], white ? Colour.White : Colour.Black));
            white = !white;
        }
        _moves = moveRecords.ToArray();

        List<RoundRecord> roundRecords = new List<RoundRecord>();
        for (int i = 0; i < _moves.Length; i += 2)
        {
            MoveRecord whiteMove = _moves[i];
            MoveRecord blackMove = (i + 1 < _moves.Length) ? _moves[i + 1] : null;
            roundRecords.Add(new RoundRecord((i / 2) + 1, whiteMove, blackMove));
        }
        _rounds = roundRecords.ToArray();
    }
}
