
namespace PathIndex.Application.Commands
{
    internal static class ListCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            for (int i = 0; i < appState.Entries.Count; i++)
            {
                Console.WriteLine(appState.Entries[i].Id + " | " + appState.Entries[i].Name + " | " + appState.Entries[i].TargetPath + (appState.Entries[i].Note != null ? " | " + appState.Entries[i].Note : "") + (appState.Entries[i].Tags.Count != 0 ? " | (" + appState.Entries[i].Tags.Count + " tags)" : ""));
            }
            Console.WriteLine();
            return new CommandResult(true, [], false);
        }
    }
}
