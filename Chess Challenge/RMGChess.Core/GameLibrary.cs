using System.Data;
using System.Reflection;

namespace RMGChess.Core;

public static class GameLibrary
{
    private static IList<GameRecord> _lichess;

    public static IList<GameRecord> LiChess => _lichess ??= GetGamesFromEmbeddedPGN("lichess_DannyTheDonkey_2023-07-20.pgn");

    private static IList<GameRecord> GetGamesFromEmbeddedPGN(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(str => str.EndsWith(resourcePath));

        if (resourceName is not null)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return PGNConverter.GetGameRecordsFromPGN(result);
                }
            }
        }

        return new List<GameRecord>();
    }
}
