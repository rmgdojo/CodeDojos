namespace RMGChess.Core;

public class GameRecord
{
    public string Name { get; init; }
    public string[] Moves { get; init; }

    public GameRecord(string name, string[] moves)
    {
        Name = name;
        Moves = moves;
    }
}