using RMGChess.Core.Resources;
using System.Data;
using System.Reflection;

namespace RMGChess.Core;

public static class GameLibrary
{
    private static IList<GameRecord> _lichess;

    public static IList<GameRecord> LiChess => _lichess ??= 
        GetGamesFromPGNString(ResourceHandler.ResourceToString("lichess_DannyTheDonkey_2023-07-20.pgn"));

    private static IList<GameRecord> GetGamesFromPGNString(string pgnData)
    {
        return PGNConverter.GetGameRecordsFromPGN(pgnData);
    }
}
