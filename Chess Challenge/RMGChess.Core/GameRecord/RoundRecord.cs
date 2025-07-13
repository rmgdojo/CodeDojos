namespace RMGChess.Core;

public class RoundRecord
{
    public int RoundIndex { get; init; }
    public MoveRecord WhiteMove { get; init; }
    public MoveRecord BlackMove { get; init; }
    public MoveRecord[] Moves => new[] { WhiteMove, BlackMove };

    public override string ToString()
    {
        return WhiteMove.MoveAsAlgebra + (BlackMove != null ? " " + BlackMove.MoveAsAlgebra : "");
    }

    public RoundRecord(int roundIndex, MoveRecord whiteMove, MoveRecord blackMove)
    {
        RoundIndex = roundIndex;
        WhiteMove = whiteMove;
        BlackMove = blackMove;
    }
}
