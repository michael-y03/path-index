
namespace PathIndex.Application.Commands
{
    internal static class CommandDispatcher
    {
        private static readonly Dictionary<string, Action<string[], AppState>> _handlers = new(StringComparer.OrdinalIgnoreCase)
        {
            { "add", AddCommand.Execute },
            { "edit", EditCommand.Execute },
            { "filter", FilterCommand.Execute },
            { "find", FindCommand.Execute },
            { "help", HelpCommand.Execute },
            { "info", InfoCommand.Execute },
            { "list", ListCommand.Execute },
            { "load", LoadCommand.Execute },
            { "remove", RemoveCommand.Execute },
            { "save", SaveCommand.Execute },
            { "tag", TagCommand.Execute },
            { "tags", TagsCommand.Execute },
            { "untag", UntagCommand.Execute },
        };

        public static bool Dispatch(string[] tokens, AppState appState)
        {
            if (tokens.Length > 0)
            {
                string command = tokens[0];
                string[] commandArgs = tokens.Length > 1 ? tokens[1..] : [];
                if (command.ToLowerInvariant() is "quit" or "exit")
                    return true;
                if (_handlers.TryGetValue(command, out var handler))
                {
                    handler(commandArgs, appState);
                    return false;
                }
                Console.WriteLine("Unknown command: '" + command + "'. Type 'help' to see available commands.\n");
            }
            return false;
        }
    }
}
