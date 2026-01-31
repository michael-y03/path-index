using PathIndex.Domain;
using PathIndex.Infrastructure.Persistence;
using System.Text.Json;

namespace PathIndex.Application.Commands
{
    internal static class LoadCommand
    {
        public static void Execute(string[] args, AppState appState)
        {
            try
            {
                String saveFilePath = SaveFilePaths.GetDefaultSaveFilePath();
                if (File.Exists(saveFilePath))
                {
                    VerifyAndLoadSaveFile(saveFilePath, appState);
                    return;
                }
                Console.WriteLine("No save found.\n");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Load failed: could not load file.\n");
            }
        }

        private static void VerifyAndLoadSaveFile(string saveFilePath, AppState appState)
        {
            string saveFile = File.ReadAllText(saveFilePath);
            SaveFileDto? saveFileDto = JsonSerializer.Deserialize<SaveFileDto>(saveFile);

            if (saveFileDto is null)
                Console.WriteLine("Save file invalid.\n");
            else
            {
                List<EntryDto> entriesToLoad = saveFileDto.Entries is null ? [] : saveFileDto.Entries;
                List<Entry> loadedEntries = [];

                foreach (EntryDto Dtoentry in entriesToLoad)
                {
                    List<string> tags = Dtoentry.Tags is null ? [] : Dtoentry.Tags;
                    Entry entry = new(Dtoentry.Id, Dtoentry.TargetPath, Dtoentry.Name, Dtoentry.Note) { Tags = tags };
                    loadedEntries.Add(entry);
                }
                appState.ReplaceState(saveFileDto.LastIssuedId, loadedEntries);
                Console.WriteLine("Save loaded.\n");
            }
            return;
        }
    }
}
