
using PathIndex.Application.Commands.CommonHelpers;

namespace PathIndex.Application.Commands.CommandHelpers
{
    internal static class EntryIdHelpers
    {
        public static EntryIdLookupResult TryGetEntryIndexById(string[] args, List<string> usage, AppState appState)
        {
            if (appState.Entries.Count == 0)
            {
                return EntryIdLookupResult.Fail(["No entries yet. Add one with: add <path>."]);
            }

            if (args.Length == 0)
            {
                return EntryIdLookupResult.Fail(usage);
            }

            if (!int.TryParse(args[0], out int id))
            {
                return EntryIdLookupResult.Fail(["Invalid id: '" + args[0] + "'. id must be a number."]);
            }

            for (int i = 0; i < appState.Entries.Count; i++)
            {
                if (appState.Entries[i].Id == id)
                    return EntryIdLookupResult.Found(i);
            }
            return EntryIdLookupResult.Fail(["No entry at " + id + ". Use 'list' to see valid IDs."]);
        }
    }
}
