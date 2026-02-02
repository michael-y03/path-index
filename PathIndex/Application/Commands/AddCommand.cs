using PathIndex.Domain;
using PathIndex.Infrastructure.FileSystem;

namespace PathIndex.Application.Commands
{
    internal static class AddCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            if (args.Length == 0 || args.Length > 3)
            {
                Console.WriteLine("Usage: add <path> [name] [note]\n");
                return;
            }
            string? name = null;
            if (args.Length >= 2)
                name = args[1];


            if (PathNameResolver.TryGetDefaultNameFromPath(args[0]) is string n)
            {
                name ??= n;
            }
            else
            {
                Console.WriteLine("Path not found: '" + args[0] + "'. The path must already exist.\n");
                return;
            }
            string? note = args.Length > 2 ? args[2] : null;
            int newId = appState.IssueNextId();
            Entry e = new(newId, args[0], name, note);
            appState.AddEntry(e);
            Console.WriteLine("Added: " + e.Name + "\n");
        }
    }
}
