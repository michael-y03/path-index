using PathIndex.Application.Commands.CommandHelpers;

namespace PathIndex.Application.Commands
{
    internal static class RemoveCommand
    {
        public static void Execute(string[] args, AppState appState)
        {
            int? nullableIndex = EntryIdHelpers.TryGetEntryIndexById(args, "Usage: remove <id>", appState);
            if (nullableIndex is int index)
            {
                string removedName = appState.Entries[index].Name;
                int removedId = appState.Entries[index].Id;
                appState.RemoveEntryById(removedId);
                Console.WriteLine("Removed entry " + removedId + ": " + removedName + "\n");
            }
        }
    }
}
