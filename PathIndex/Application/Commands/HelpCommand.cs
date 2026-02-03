namespace PathIndex.Application.Commands
{
    internal static class HelpCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            List<string> resultLines =
            [
                "Commands:",
                "  help                          Show this help",
                "  add <path> [name] [note]      Add an entry",
                "  remove <id>                   Remove an entry",
                "  edit <id> clear-note          Clear the note for an entry",
                "  edit <id> <name|note> <value> Edit one field (name/note) for an entry",
                "  tag <id> <tag>                Add a tag to an entry",
                "  untag <id> <tag>              Remove a tag from an entry",
                "  tags <id>                     Show tags for one entry",
                "  find <text>                   List entries containing text",
                "  filter tag <tag>              List entries with matching tag",
                "  list                          List entries",
                "  info <id>                     Show full details for one entry",
                "  quit / exit                   Close PathIndex",
                "  save                          save PathIndex entries",
                "  load                          load PathIndex entries",
            ];
            return new CommandResult(true, resultLines);
        }
    }
}
