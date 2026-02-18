using PathIndex.Domain;
using PathIndex.Infrastructure.Persistence;
using System.Text.Json;

namespace PathIndex.Application.Commands
{
    internal static class SaveCommand
    {
        private static readonly JsonSerializerOptions CachedJsonOptions = new() { WriteIndented = true };
        public static CommandResult Execute(string[] args, AppState appState)
        {
            List<EntryDto> entryDtos = [];
            foreach (Entry entry in appState.Entries)
            {
                EntryDto entryDto = new(entry.Id, entry.TargetPath, entry.Name, entry.Note, entry.Tags);
                entryDtos.Add(entryDto);
            }
            SaveFileDto saveFileDto = new(appState.LastIssuedId, entryDtos);

            try
            {
                string saveFilePath = SaveFilePaths.GetDefaultSaveFilePath();

                string saveData = JsonSerializer.Serialize(saveFileDto, CachedJsonOptions);
                saveData += Environment.NewLine;

                File.WriteAllText(saveFilePath, saveData);
                return new CommandResult(true, ["Saved " + entryDtos.Count + " entries to " + saveFilePath]);
            }
            catch (Exception e)
            {
                return new CommandResult(false, ["Save failed: could not write file. " + e.Message]);
            }
        }
    }
}
