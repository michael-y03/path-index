
namespace PathIndex.Application.Commands
{
    internal static class ListCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            List<string> resultLines = [];
            for (int i = 0; i < appState.Entries.Count; i++)
            {
                resultLines.Add(appState.Entries[i].Id + " | " + appState.Entries[i].Name + " | " + appState.Entries[i].TargetPath + (appState.Entries[i].Note != null ? " | " + appState.Entries[i].Note : "") + (appState.Entries[i].Tags.Count != 0 ? " | (" + appState.Entries[i].Tags.Count + " tags)" : ""));
            }
            if (!resultLines.Any())
                resultLines.Add("No entries yet.");

            return new CommandResult(true, resultLines);
        }
    }
}
