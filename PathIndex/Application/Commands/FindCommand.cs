using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class FindCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: find <text>";
            if (args.Length != 1)
            {
                return new CommandResult(false, [usage]);
            }

            string text = args[0].ToLowerInvariant();
            List<Entry> found = [];
            foreach (Entry entry in appState.Entries)
            {
                if (entry.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(entry);
                }
                else if (entry.TargetPath.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(entry);
                }
                else if (entry.Note != null && entry.Note.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(entry);
                }
            }

            List<string> resultLines = [];
            resultLines.Add(found.Count > 0 ? "Found " + found.Count + " entries matching " + text + "." : "Found 0 entries matching " + text + ".");
            for (int i = 0; i < found.Count; i++)
            {
                resultLines.Add("Entry " + found[i].Id + " | " + found[i].Name + " | " + found[i].TargetPath + (found[i].Note != null ? " | " + found[i].Note : "") + (found[i].Tags.Count != 0 ? " | (" + found[i].Tags.Count + " tags)" : ""));
            }
            return new CommandResult(true, resultLines);
        }
    }
}
