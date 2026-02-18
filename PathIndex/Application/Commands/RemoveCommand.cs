using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Application.Commands.CommonHelpers;

namespace PathIndex.Application.Commands
{
    internal static class RemoveCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            EntryIdLookupResult result = EntryIdHelpers.TryGetEntryIndexById(args, ["Usage: remove <id>"], appState);
            if (result.Index is int index)
            {
                string removedName = appState.Entries[index].Name;
                int removedId = appState.Entries[index].Id;
                appState.RemoveEntryById(removedId);
                return new CommandResult(true, ["Removed entry " + removedId + ": " + removedName]);
            }
            return new CommandResult(false, result.Lines);
        }
    }
}
