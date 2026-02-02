namespace PathIndex.Application.Commands
{
    internal static class HelpCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  help                          Show this help");
            Console.WriteLine("  add <path> [name] [note]      Add an entry");
            Console.WriteLine("  remove <id>                   Remove an entry");
            Console.WriteLine("  edit <id> clear-note          Clear the note for an entry");
            Console.WriteLine("  edit <id> <name|note> <value> Edit one field (name/note) for an entry");
            Console.WriteLine("  tag <id> <tag>                Add a tag to an entry");
            Console.WriteLine("  untag <id> <tag>              Remove a tag from an entry");
            Console.WriteLine("  tags <id>                     Show tags for one entry");
            Console.WriteLine("  find <text>                   List entries containing text");
            Console.WriteLine("  filter tag <tag>              List entries with matching tag");
            Console.WriteLine("  list                          List entries");
            Console.WriteLine("  info <id>                     Show full details for one entry");
            Console.WriteLine("  quit / exit                   Close PathIndex\n");
            Console.WriteLine("  save                          save PathIndex entries\n");
            Console.WriteLine("  load                          load PathIndex entries\n");
        }
    }
}
