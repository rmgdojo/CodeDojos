namespace RMGChess.Core;

public class MoveRecord
{
    public int MoveIndex { get; init; }
    public float RoundIndex { get; init; }
    public string MoveAsAlgebra { get; init; }
    public Colour WhoseTurn { get; init; }
    public bool IsWhite => WhoseTurn == Colour.White;
    public bool IsBlack => WhoseTurn == Colour.Black;

    public override string ToString()
    {
        return $"{WhoseTurn}: {MoveAsAlgebra}";
    }

    public Move GetMove(Game game)
    {
        return Algebra.DecodeAlgebra(MoveAsAlgebra, game.Board, WhoseTurn);
    }

    public MoveRecord(int moveIndex, string moveAsAlgebra, Colour whoseTurn)
    {
        MoveIndex = moveIndex;
        RoundIndex = (moveIndex / 2f) + 0.5f;
        MoveAsAlgebra = moveAsAlgebra;
        WhoseTurn = whoseTurn;
    }
}
