using RMGChess.Core.Resources;
using System.Data;
using System.Reflection;

namespace RMGChess.Core;

public static class GameLibrary
{
    private static IReadOnlyList<GameRecord> _magnusCarlsenGames;

    public static IReadOnlyList<GameRecord> MagnusCarlsenGames => _magnusCarlsenGames ??= 
        GetGamesFromPGNString(ResourceHandler.ResourceToString("lichess_DannyTheDonkey_2023-07-20.pgn"));

    private static IReadOnlyList<GameRecord> GetGamesFromPGNString(string pgnData)
    {
        return PGNConverter.GetGameRecordsFromPGN(pgnData).AsReadOnly();
    }
}
