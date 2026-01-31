using System;
using System.Collections.Generic;
using System.Text;

namespace PathIndex.Infrastructure.FileSystem
{
    internal static class PathNameResolver
    {
        public static string? TryGetDefaultNameFromPath(string path)
        {
            if (Path.GetPathRoot(path) == Path.GetFullPath(path))
                return Path.GetPathRoot(path);

            while (path.EndsWith('/') || path.EndsWith('\\'))
                path = path[..^1];

            string? folderName;
            if (Directory.Exists(path))
            {
                folderName = Path.GetFileName(path);
                return folderName;
            }

            if (File.Exists(path))
            {
                folderName = Path.GetFileName(Path.GetDirectoryName(path));
                return folderName;
            }

            return null;
        }
    }
}
