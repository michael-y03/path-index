
namespace PathIndex.Application.Commands.CommandHelpers
{
    internal static class EntryIdHelpers
    {
        public static int? TryGetEntryIndexById(string[] args, string usage, AppState appState)
        {
            if (appState.Entries.Count == 0)
            {
                Console.WriteLine("No entries yet. Add one with: add <path>.\n");
                return null;
            }

            if (args.Length == 0)
            {
                Console.WriteLine(usage + "\n");
                return null;
            }

            if (!int.TryParse(args[0], out int id))
            {
                Console.WriteLine("Invalid id: '" + args[0] + "'.\nid must be a number.\n");
                return null;
            }

            for (int i = 0; i < appState.Entries.Count; i++)
            {
                if (appState.Entries[i].Id == id)
                    return i;
            }
            Console.WriteLine("No entry at " + id + ". Use 'list' to see valid IDs.\n");
            return null;
        }
    }
}
