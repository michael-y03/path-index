using PathIndex.Domain;
using PathIndex.Infrastructure.Persistence;
using System.Text.Json;

namespace PathIndex.Application.Commands
{
    internal static class LoadCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            try
            {
                string saveFilePath = SaveFilePaths.GetDefaultSaveFilePath();
                if (File.Exists(saveFilePath))
                {
                    bool success = VerifyAndLoadSaveFile(saveFilePath, appState);
                    if (success)
                        return new CommandResult(true, ["Save loaded."]);
                    return new CommandResult(false, ["Save file invalid."]);
                }
                return new CommandResult(true, ["No save found."]);
            }
            catch (Exception)
            {
                return new CommandResult(false, ["Load failed: could not load file."]);
            }
        }

        private static bool VerifyAndLoadSaveFile(string saveFilePath, AppState appState)
        {
            string saveFile = File.ReadAllText(saveFilePath);
            SaveFileDto? saveFileDto = JsonSerializer.Deserialize<SaveFileDto>(saveFile);

            if (saveFileDto is null)
                return false;
            else
            {
                List<EntryDto> entriesToLoad = saveFileDto.Entries is null ? [] : saveFileDto.Entries;
                List<Entry> loadedEntries = [];

                foreach (EntryDto dtoEntry in entriesToLoad)
                {
                    IEnumerable<string> tags = dtoEntry.Tags is null ? [] : dtoEntry.Tags;
                    Entry entry = new(dtoEntry.Id, dtoEntry.TargetPath, dtoEntry.Name, dtoEntry.Note, tags) ;
                    loadedEntries.Add(entry);
                }
                appState.ReplaceState(saveFileDto.LastIssuedId, loadedEntries);
                return true;
            }
        }
    }
}
