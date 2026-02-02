using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class TagCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: tag <id> <tag>\n";
            if (args.Length != 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = EntryIdHelpers.TryGetEntryIndexById(args, usage, appState);
            if (nullableIndex is not int index)
                return;

            Entry entry = appState.Entries[index];

            string tag = args[1].ToLowerInvariant();
            if (!entry.Tags.Contains(tag))
            {
                entry.Tags.Add(tag);
                Console.WriteLine("Entry " + entry.Id + " (" + entry.Name + ") added Tag: " + tag + "\n");
            }
            else
                Console.WriteLine("Tag: " + tag + " already exists" + " on entry " + entry.Id + ".\n");
        }
    }
}
