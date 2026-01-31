
namespace PathIndex.Infrastructure.Persistence
{
    internal static class SaveFilePaths
    {
        public static string GetDefaultSaveFilePath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolderPath = Path.Combine(folderPath, "PathIndex");
            Directory.CreateDirectory(appFolderPath);
            return Path.Combine(appFolderPath, "pathindex.json");
        }
    }
}
