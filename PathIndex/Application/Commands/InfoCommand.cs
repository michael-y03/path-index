using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Application.Commands.CommonHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class InfoCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            EntryIdLookupResult result = EntryIdHelpers.TryGetEntryIndexById(args, "Usage: info <id>", appState);
            if (result.Index is int index)
            {
                Entry entry = appState.Entries[index];
                string tags = string.Join(", ", entry.Tags);
                List<string> resultLines =
                [
                    "Entry " + entry.Id,
                    "Name: " + entry.Name,
                    "Path: " + entry.TargetPath
                ];
                if (entry.Note != null)
                    resultLines.Add("Note: " + entry.Note);
                if (entry.Tags.Count != 0)
                    resultLines.Add("Tags: " + tags);
                return new CommandResult(true, resultLines);
            }
            return new CommandResult(false, result.Lines);
        }
    }
}
