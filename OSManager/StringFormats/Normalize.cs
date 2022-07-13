using System.IO;

namespace OSManager.StringFormats
{
    public static class Normalize
    {
        public static string NormalizePath(string badPath)
        {
            string filePath;
            if (!string.IsNullOrEmpty(badPath))
            {
                if (badPath[0] != '"' && badPath.IndexOf(" /") == -1 && badPath.IndexOf(" -") == -1)
                {
                    filePath = Path.GetFullPath(badPath);
                    return filePath;
                }
                else if (badPath[0] != '"' && badPath.IndexOf(" /") != -1)
                {
                    filePath = badPath.Substring(badPath.IndexOf(badPath[0]), badPath.LastIndexOf('/'));
                    return filePath;
                }
                else if (badPath[0] != '"' && badPath.IndexOf(" -") != -1)
                {
                    filePath = badPath.Substring(badPath.IndexOf(badPath[0]), badPath.IndexOf(" -"));
                    return filePath;
                }
                else if (badPath[0] == '"')
                {
                    filePath = badPath.Substring(badPath.IndexOf('"') + 1, badPath.LastIndexOf('"') - 1);
                    return filePath;
                }
                else
                {
                    return badPath;
                }
            }
            else
            {
                return badPath;
            }
        }
    }
}
