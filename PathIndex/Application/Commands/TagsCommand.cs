using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class TagsCommand
    {
        public static void Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: tags <id>\n";
            if (args.Length != 1)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = EntryIdHelpers.TryGetEntryIndexById(args, usage, appState);
            if (nullableIndex is not int index)
                return;

            Entry entry = appState.Entries[index];

            string tags = string.Join(", ", entry.Tags);
            Console.WriteLine("Entry " + entry.Id + " (" + entry.Name + ")" + (entry.Tags.Count != 0 ? "\nTags: " + tags : "\nTags: (None)") + "\n");
        }
    }
}
