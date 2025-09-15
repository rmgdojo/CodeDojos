using System.Reflection;

namespace RMGChess.Core.Resources
{
    public static class ResourceHandler
    {
        public static string ResourceToString(string resourcePath)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().SingleOrDefault(str => str.EndsWith(resourcePath));

            if (resourceName is not null)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return string.Empty;
        }
    }
}
