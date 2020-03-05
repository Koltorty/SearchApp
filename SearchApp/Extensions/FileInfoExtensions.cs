using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SearchApp.Extensions
{
    public static class FileInfoExtensions
    {
        public static IEnumerable<FileInfo> FilterOnTextFiles(this IEnumerable<FileInfo> files)
        {
            return files.Where(file =>
                string.Equals(file.Extension, ".txt") || string.Equals(file.Extension, ".doc") ||
                string.Equals(file.Extension, ".docx"));
        }
    }
}
