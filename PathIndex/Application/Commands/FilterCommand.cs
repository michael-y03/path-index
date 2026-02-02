using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class FilterCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: filter tag <tag>\n";
            if (args.Length != 2 || !args[0].Equals("tag", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine(usage);
                return;
            }

            string userTag = args[1].ToLowerInvariant();
            List<Entry> found = [];
            foreach (Entry entry in appState.Entries)
            {
                if (entry.Tags.Contains(userTag))
                {
                    found.Add(entry);
                }
            }

            Console.WriteLine(found.Count > 0 ? "Found " + found.Count + " entries matching tag " + userTag + "." : "Found 0 entries matching tag " + userTag + ".");
            for (int i = 0; i < found.Count; i++)
            {
                Console.WriteLine("Entry " + found[i].Id + " | " + found[i].Name + " | " + found[i].TargetPath + (found[i].Note != null ? " | " + found[i].Note : "") + (found[i].Tags.Count != 0 ? " | (" + found[i].Tags.Count + " tags)" : ""));
            }
            Console.WriteLine();
        }
    }
}
