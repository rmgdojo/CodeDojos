using System.Reflection;
using RMGChess.Core.Converters;

namespace RMGChess.Core;

public static class PGNGames
{
    public static List<Dictionary<string, string[]>> DannyTheDonkey =>
        GetPGNFromEmbeddedResource("lichess_DannyTheDonkey_2023-07-20.pgn");

    private static List<Dictionary<string, string[]>> GetPGNFromEmbeddedResource(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(resourcePath));

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string result = reader.ReadToEnd();
            return PGNConverter.ConvertGames(result);
        }

        return new List<Dictionary<string, string[]>>();
    }
}