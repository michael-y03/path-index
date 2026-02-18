using PathIndex.Domain;
using PathIndex.Infrastructure.FileSystem;

namespace PathIndex.Application.Commands
{
    internal static class AddCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            if (args.Length == 0 || args.Length > 3)
                return new CommandResult(false, ["Usage: add <path> [name] [note]"]);
            string? name = null;
            if (args.Length >= 2)
                name = args[1];


            if (PathNameResolver.TryGetDefaultNameFromPath(args[0]) is string n)
                name ??= n;
            else
                return new CommandResult(false, ["Path not found: '" + args[0] + "'. The path must already exist."]);
            string? note = args.Length > 2 ? args[2] : null;
            int newId = appState.IssueNextId();
            Entry e = new(newId, args[0], name, note);
            appState.AddEntry(e);
            return new CommandResult(true, ["Added: " + e.Name]);

        }
    }
}
