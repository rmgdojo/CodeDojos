using RMGChess.Core.Resources;

namespace RMGChess.Core;

public static class GameLibrary
{
    public static IReadOnlyList<GameRecord> MagnusCarlsenGames =>
        field ??= GetGamesFromPGNString(ResourceHandler.ResourceToString("lichess_DannyTheDonkey_2023-07-20.pgn"));

    private static IReadOnlyList<GameRecord> GetGamesFromPGNString(string pgnData) =>
        PGNConverter.GetGameRecordsFromPGN(pgnData).AsReadOnly();
}
