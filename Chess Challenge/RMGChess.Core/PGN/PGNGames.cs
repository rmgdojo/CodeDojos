using System.Data;
using System.Reflection;

namespace RMGChess.Core;

public static class PGNGames
{
    private static IList<GameRecord> _dannyTheDonkey;

    public static IList<GameRecord> DannyTheDonkey => _dannyTheDonkey ??=
        GetPGNFromEmbeddedResource("lichess_DannyTheDonkey_2023-07-20.pgn");

    private static IList<GameRecord> GetPGNFromEmbeddedResource(string resourcePath)
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
                    return PGNConverter.ConvertGames(result);
                }
            }
        }

        return new List<GameRecord>();
    }
}
