using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class FilterCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: filter tag <tag>";
            if (args.Length != 2 || !args[0].Equals("tag", StringComparison.InvariantCultureIgnoreCase))
            {
                return new CommandResult(false, [usage]);
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

            List<string> resultLines = [];
            resultLines.Add(found.Count > 0 ? "Found " + found.Count + " entries matching tag " + userTag + "." : "Found 0 entries matching tag " + userTag + ".");
            for (int i = 0; i < found.Count; i++)
            {
                resultLines.Add("Entry " + found[i].Id + " | " + found[i].Name + " | " + found[i].TargetPath + (found[i].Note != null ? " | " + found[i].Note : "") + (found[i].Tags.Count != 0 ? " | (" + found[i].Tags.Count + " tags)" : ""));
            }
            return new CommandResult(true, resultLines);
        }
    }
}
