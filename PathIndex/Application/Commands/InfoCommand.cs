using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class InfoCommand
    {
        public static void Execute(string[] args, AppState appState)
        {
            int? nullableIndex = EntryIdHelpers.TryGetEntryIndexById(args, "Usage: info <id>", appState);
            if (nullableIndex is int index)
            {
                Entry entry = appState.Entries[index];
                string tags = string.Join(", ", entry.Tags);
                Console.WriteLine("Entry " + entry.Id + "\nName: " + entry.Name + "\nPath: " + entry.TargetPath + (entry.Note != null ? "\nNote: " + entry.Note : "") + (entry.Tags.Count != 0 ? "\nTags: " + tags : "") + "\n");
            }
        }
    }
}
