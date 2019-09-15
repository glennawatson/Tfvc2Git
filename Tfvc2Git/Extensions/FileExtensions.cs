using System.IO;

namespace Tfvc2Git.Extensions
{
    public static class FileExtensions
    {
        public static void CreateFileDirectory(this string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
