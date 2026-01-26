

namespace PathIndex
{
    class Program
    {
        static List<Entry> entries = new List<Entry>();

        static void Main(string[] args)
        {
            Console.WriteLine("PathIndex — index and annotate local folders");
            Console.WriteLine("Type 'help' to see commands.\n");

            while (true)
            {
                string? input = "";
                while (input.Length == 0)
                {
                    Console.Write("> ");
                    input = Console.ReadLine();
                    if (input is null)
                        return;
                    input = input.Trim();
                }

                string[] tokens = TokenizeInput(input);

                string command = tokens[0].ToLowerInvariant();
                string[] commandArgs = tokens.Length > 1 ? tokens[1..] : Array.Empty<string>();

                switch (command)
                {
                    case "quit":
                        return;
                    case "exit":
                        return;
                    case "help":
                        HelpCommand();
                        break;
                    case "add":
                        AddCommand(commandArgs);
                        break;
                    case "remove":
                        RemoveCommand(commandArgs);
                        break;
                    case "edit":
                        EditCommand(commandArgs);
                        break;
                    case "tag":
                        TagCommand(commandArgs);
                        break;
                    case "untag":
                        UntagCommand(commandArgs);
                        break;
                    case "tags":
                        TagsCommand(commandArgs);
                        break;
                    case "info":
                        InfoCommand(commandArgs);
                        break;
                    case "list":
                        ListCommand();
                        break;
                    default:
                        Console.WriteLine("Unknown command: '" + command + "'. Type 'help' to see available commands.\n");
                        break;
                }
            }
        }

        static string[] TokenizeInput(string input)
        {
            string[] tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return tokens;
        }

        static void HelpCommand()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  help                             Show this help");
            Console.WriteLine("  add <path> [name] [note]         Add an entry (spaces aren't supported in arguments yet)");
            Console.WriteLine("  remove <index>                   Remove an entry");
            Console.WriteLine("  edit <index> clear-note          Clear the note for an entry");
            Console.WriteLine("  edit <index> <name|note> <value> Edit one field (name/note) for an entry");
            Console.WriteLine("  tag <id> <tag>                   Add a tag to an entry");
            Console.WriteLine("  untag <id> <tag>                 remove a tag from an entry");
            Console.WriteLine("  tags <id>                        Show tags for one entry");
            Console.WriteLine("  list                             List entries");
            Console.WriteLine("  info <index>                     Show full details for one entry");
            Console.WriteLine("  quit / exit                      Close PathIndex\n");
        }

        static void AddCommand(string[] args)
        {
            if (args.Length == 0 || args.Length > 3)
            {
                Console.WriteLine("Usage: add <path> [name] [note]\nNote: spaces aren’t supported in arguments yet.\n");
                return;
            }
            string? name = null;
            if (args.Length >= 2)
                name = args[1];


            if (TryGetDefaultNameFromPath(args[0]) is string n)
            {
                if (name is null)
                    name = n;
            }
            else
            {
                Console.WriteLine("Path not found: '" + args[0] + "'. The path must already exist.\n");
                return;
            }
            string? note = args.Length > 2 ? args[2] : null;
            Entry e = new Entry(args[0], name, note);
            entries.Add(e);
            Console.WriteLine("Added: " + e.Name + "\n");
        }

        private static string? TryGetDefaultNameFromPath(string path)
        {
            if (Path.GetPathRoot(path) == Path.GetFullPath(path))
                return Path.GetPathRoot(path);

            while (path.EndsWith('/') || path.EndsWith('\\'))
                path = path.Remove(path.Length - 1);

            string? folderName;
            if (Directory.Exists(path))
            {
                folderName = Path.GetFileName(path);
                return folderName;
            }

            if (File.Exists(path))
            {
                folderName = Path.GetFileName(Path.GetDirectoryName(path));
                return folderName;
            }

            return null;
        }

        static void ListCommand()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine(i + 1 + " | " + entries[i].Name + " | " + entries[i].TargetPath + (entries[i].Note != null ? " | " + entries[i].Note : "") + (entries[i].Tags.Count != 0 ? " | (" + entries[i].Tags.Count + " tags)" : ""));
            }
            Console.WriteLine();
        }

        private static void InfoCommand(string[] args)
        {
            int? nullableIndex = TryGetEntryZeroBasedIndex(args, "Usage: info <index>");
            if (nullableIndex is int index)
            {
                Entry entry = entries[index];
                string tags = string.Join(", ", entry.Tags);
                Console.WriteLine("Entry " + (index + 1) + "\nName: " + entry.Name + "\nPath: " + entry.TargetPath + (entry.Note != null ? "\nNote: " + entry.Note : "") + (entry.Tags.Count != 0 ? "\nTags: " + tags : "") + "\n");
            }
        }

        private static void RemoveCommand(string[] args)
        {
            int? nullableIndex = TryGetEntryZeroBasedIndex(args, "Usage: remove <index>");
            if (nullableIndex is int index)
            {
                string removedName = entries[index].Name;
                entries.RemoveAt(index);
                Console.WriteLine("Removed entry " + (index + 1) + ": " + removedName + "\n");
            }
        }

        private static void EditCommand(string[] args)
        {
            const string usage = "Usage: edit <index> name <value>\n       edit <index> note <value>\n       edit <index> clear-note\n";
            if (args.Length < 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryZeroBasedIndex(args, usage);
            if (nullableIndex is not int index)
                return;
            Entry entry = entries[index];
            string field = args[1].ToLowerInvariant();
            if (field != "clear-note" && field != "name" && field != "note")
            {
                Console.WriteLine("Invalid field: '" + field + "'.\nValid fields: name, note, clear-note.\n" + usage);
                return;
            }

            if (field == "clear-note")
            {
                if (args.Length != 2)
                {
                    Console.WriteLine(usage);
                    return;
                }
                ClearNote(entry);
                return;
            }
            if (args.Length != 3)
            {
                Console.WriteLine(usage);
                return;
            }
            if (field == "name")
            {
                UpdateName(entry, args[2]);
                return;
            }
            if (field == "note")
            {
                UpdateNote(entry, args[2]);
                return;
            }
        }

        private static void UpdateName(Entry entry, string newName)
        {
            string originalName = entry.Name;
            entry.Name = newName;
            Console.WriteLine("Updated name: " + originalName + " to " + newName + "\n");
        }

        private static void UpdateNote(Entry entry, string newNote)
        {
            string originalNote = "(" + (entry.Note == null ? "none" : entry.Note) + ")";
            entry.Note = newNote;
            Console.WriteLine("Updated note: " + originalNote + " -> " + newNote + "\n");
        }

        private static void ClearNote(Entry entry)
        {
            string? originalNote = entry.Note;
            entry.Note = null;
            Console.WriteLine(originalNote == null ? "Nothing to clear.\n" : "Cleared note: (" + originalNote + ")\n");
        }

        private static int? TryGetEntryZeroBasedIndex(string[] args, string usage)
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No entries yet. Add one with: add <path>.\n");
                return null;
            }

            if (args.Length == 0)
            {
                Console.WriteLine(usage + "\nIndex must be a number between 1 and " + entries.Count + ".\n");
                return null;
            }

            if (!int.TryParse(args[0], out int index))
            {
                Console.WriteLine("Invalid index: '" + args[0] + "'.\nIndex must be a number.\n");
                return null;
            }

            if (index < 1 || index > entries.Count)
            {
                Console.WriteLine("Index out of range. Valid range: 1–" + entries.Count + ".\nNo entry at " + index + ". Use 'list' to see valid indices.\n");
                return null;
            }
            return index - 1;
        }

        private static void TagCommand(string[] args)
        {
            const string usage = "Usage: tag <index> <tag>\n";
            if (args.Length != 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryZeroBasedIndex(args, usage);
            if (nullableIndex is not int index)
                return;

            Entry entry = entries[index];

            string tag = args[1].ToLowerInvariant();
            if (!entry.Tags.Contains(tag))
            {
                entry.Tags.Add(tag);
                Console.WriteLine("Entry " + (index + 1) + " (" + entry.Name + ") added Tag: " + tag + "\n");
            }
            else
                Console.WriteLine("Tag: " + tag + " already exists" + " on entry " + (index + 1) + ".\n");
        }

        private static void UntagCommand(string[] args)
        {
            const string usage = "Usage: untag <index> <tag>\n";
            if (args.Length != 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryZeroBasedIndex(args, usage);
            if (nullableIndex is not int index)
                return;

            Entry entry = entries[index];

            string tag = args[1].ToLowerInvariant();
            if (entry.Tags.Contains(tag))
            {
                entry.Tags.Remove(tag);
                Console.WriteLine("Entry " + (index + 1) + " (" + entry.Name + ") removed Tag: " + tag + "\n");
            }
            else
                Console.WriteLine("Tag: " + tag + " not found" + " on entry " + (index + 1) + ".\n");
        }

        private static void TagsCommand(string[] args)
        {
            const string usage = "Usage: tags <index>\n";
            if (args.Length != 1)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryZeroBasedIndex(args, usage);
            if (nullableIndex is not int index)
                return;

            Entry entry = entries[index];

            string tags = string.Join(", ", entry.Tags);
            Console.WriteLine("Entry " + (index + 1) + " (" + entry.Name + ")" + (entry.Tags.Count != 0 ? "\nTags: " + tags : "\nTags: (None)") + "\n");
        }

    }
}