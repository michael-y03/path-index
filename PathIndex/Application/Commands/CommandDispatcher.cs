namespace PathIndex.Application.Commands
{
    internal static class CommandDispatcher
    {
        private static readonly Dictionary<string, Func<string[], AppState, CommandResult>> _handlers = new(StringComparer.OrdinalIgnoreCase)
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

        public static CommandResult Dispatch(string[] tokens, AppState appState)
        {
            List<string> resultLines = [];
            if (tokens.Length > 0)
            {
                string command = tokens[0];
                string[] commandArgs = tokens.Length > 1 ? tokens[1..] : [];
                if (command.ToLowerInvariant() is "quit" or "exit")
                    return new CommandResult(false, [], true);
                if (_handlers.TryGetValue(command, out var handler))
                {
                    return handler(commandArgs, appState);
                }
                string lines = "Unknown command: '" + command + "'. Type 'help' to see available commands.";
                resultLines.Add(lines);
            }
            return new CommandResult(false, resultLines, false);
        }
    }
}
