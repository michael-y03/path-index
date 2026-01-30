using PathIndex.Persistence;
using System.Text.Json;

namespace PathIndex
{
    class Program
    {
        static readonly List<Entry> entries = [];
        static int lastIssuedId = 0;

        private static readonly System.Text.Json.JsonSerializerOptions CachedJsonOptions = new() { WriteIndented = true };

        static void Main()
        {
            Console.WriteLine("PathIndex — index and annotate local folders");
            Console.WriteLine("Type 'help' to see commands.\n");
            LoadCommand();

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
                if (tokens.Length > 0)
                {
                    string command = tokens[0].ToLowerInvariant();
                    string[] commandArgs = tokens.Length > 1 ? tokens[1..] : [];

                    switch (command)
                    {
                        case "quit":
                            return;
                        case "exit":
                            return;
                        case "save":
                            SaveCommand();
                            break;
                        case "load":
                            LoadCommand();
                            break;
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
                        case "find":
                            FindCommand(commandArgs);
                            break;
                        case "filter":
                            FilterCommand(commandArgs);
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
        }

        private static void SaveCommand()
        {
            List<EntryDto> entryDtos = [];
            foreach (Entry entry in entries)
            {
                EntryDto entryDto = new(entry.Id, entry.TargetPath, entry.Name, entry.Note, entry.Tags);
                entryDtos.Add(entryDto);
            }
            SaveFileDto saveFileDto = new(lastIssuedId, entryDtos);

            try
            {
                string saveFilePath = FindSavePath();

                string saveData = JsonSerializer.Serialize(saveFileDto, CachedJsonOptions);
                saveData += Environment.NewLine;

                File.WriteAllText(saveFilePath, saveData);
                Console.WriteLine("Saved " + entryDtos.Count + " entries to " + saveFilePath + "\n");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Save failed: could not write file. " + e.Message + "\n");
            }
        }

        private static void LoadCommand()
        {
            try
            {
                String saveFilePath = FindSavePath();
                if (File.Exists(saveFilePath))
                {
                    VerifyAndLoadSaveFile(saveFilePath);
                    return;
                }
                Console.WriteLine("No save found.\n");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Load failed: could not load file.\n");
            }
        }

        private static void VerifyAndLoadSaveFile(string saveFilePath)
        {
            string saveFile = File.ReadAllText(saveFilePath);
            SaveFileDto? saveFileDto = JsonSerializer.Deserialize<SaveFileDto>(saveFile);

            if (saveFileDto is null)
                Console.WriteLine("Save file invalid.\n");
            else
            {
                lastIssuedId = saveFileDto.LastIssuedId;
                List<EntryDto> entriesToLoad = saveFileDto.Entries is null ? [] : saveFileDto.Entries;

                entries.Clear();
                foreach (EntryDto Dtoentry in entriesToLoad)
                {
                    List<string> tags = Dtoentry.Tags is null ? [] : Dtoentry.Tags;
                    Entry entry = new(Dtoentry.Id, Dtoentry.TargetPath, Dtoentry.Name, Dtoentry.Note) { Tags = tags };
                    entries.Add(entry);
                }
                Console.WriteLine("Save loaded.\n");
            }
            return;
        }

        private static string FindSavePath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolderPath = Path.Combine(folderPath, "PathIndex");

            if (!Directory.Exists(appFolderPath))
            {
                Directory.CreateDirectory(appFolderPath);
            }

            string saveFilePath = Path.Combine(appFolderPath, "pathindex.json");
            return saveFilePath;
        }

        static string[] TokenizeInput(string input)
        {
            List<string> tokens = [];
            bool insideQuotedArgument = false;
            bool tokenInProgress = false;
            string argument = "";
            foreach (char character in input)
            {

                if (character == '\"')
                {
                    insideQuotedArgument = !insideQuotedArgument;
                    tokenInProgress = true;
                }
                if (insideQuotedArgument && character != '\"')
                    argument += character;
                else if (tokenInProgress && char.IsWhiteSpace(character) && !insideQuotedArgument)
                {
                    tokens.Add(argument);
                    argument = "";
                    tokenInProgress = false;
                }
                else if (character != '\"' && !char.IsWhiteSpace(character))
                {
                    argument += character;
                    tokenInProgress = true;
                }
            }
            if (!insideQuotedArgument && tokenInProgress)
                tokens.Add(argument);
            else
            {
                Console.WriteLine("Quoted argument error. \n");
                return [];
            }

            string[] allTokens = [.. tokens];
            return allTokens;
        }

        static void HelpCommand()
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

        static void AddCommand(string[] args)
        {
            if (args.Length == 0 || args.Length > 3)
            {
                Console.WriteLine("Usage: add <path> [name] [note]\n");
                return;
            }
            string? name = null;
            if (args.Length >= 2)
                name = args[1];


            if (TryGetDefaultNameFromPath(args[0]) is string n)
            {
                name ??= n;
            }
            else
            {
                Console.WriteLine("Path not found: '" + args[0] + "'. The path must already exist.\n");
                return;
            }
            string? note = args.Length > 2 ? args[2] : null;
            lastIssuedId += 1;
            Entry e = new(lastIssuedId, args[0], name, note);
            entries.Add(e);
            Console.WriteLine("Added: " + e.Name + "\n");
        }

        private static string? TryGetDefaultNameFromPath(string path)
        {
            if (Path.GetPathRoot(path) == Path.GetFullPath(path))
                return Path.GetPathRoot(path);

            while (path.EndsWith('/') || path.EndsWith('\\'))
                path = path[..^1];

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
                Console.WriteLine(entries[i].Id + " | " + entries[i].Name + " | " + entries[i].TargetPath + (entries[i].Note != null ? " | " + entries[i].Note : "") + (entries[i].Tags.Count != 0 ? " | (" + entries[i].Tags.Count + " tags)" : ""));
            }
            Console.WriteLine();
        }

        private static void InfoCommand(string[] args)
        {
            int? nullableIndex = TryGetEntryIndexById(args, "Usage: info <id>");
            if (nullableIndex is int index)
            {
                Entry entry = entries[index];
                string tags = string.Join(", ", entry.Tags);
                Console.WriteLine("Entry " + entry.Id + "\nName: " + entry.Name + "\nPath: " + entry.TargetPath + (entry.Note != null ? "\nNote: " + entry.Note : "") + (entry.Tags.Count != 0 ? "\nTags: " + tags : "") + "\n");
            }
        }

        private static void RemoveCommand(string[] args)
        {
            int? nullableIndex = TryGetEntryIndexById(args, "Usage: remove <id>");
            if (nullableIndex is int index)
            {
                string removedName = entries[index].Name;
                int removedId = entries[index].Id;
                entries.RemoveAt(index);
                Console.WriteLine("Removed entry " + removedId + ": " + removedName + "\n");
            }
        }

        private static void EditCommand(string[] args)
        {
            const string usage = "Usage: edit <id> name <value>\n       edit <id> note <value>\n       edit <id> clear-note\n";
            if (args.Length < 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryIndexById(args, usage);
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
            string originalNote = "(" + (entry.Note ?? "none") + ")";
            entry.Note = newNote;
            Console.WriteLine("Updated note: " + originalNote + " -> " + newNote + "\n");
        }

        private static void ClearNote(Entry entry)
        {
            string? originalNote = entry.Note;
            entry.Note = null;
            Console.WriteLine(originalNote == null ? "Nothing to clear.\n" : "Cleared note: (" + originalNote + ")\n");
        }

        private static int? TryGetEntryIndexById(string[] args, string usage)
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No entries yet. Add one with: add <path>.\n");
                return null;
            }

            if (args.Length == 0)
            {
                Console.WriteLine(usage + "\n");
                return null;
            }

            if (!int.TryParse(args[0], out int id))
            {
                Console.WriteLine("Invalid id: '" + args[0] + "'.\nid must be a number.\n");
                return null;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Id == id)
                    return i;
            }
            Console.WriteLine("No entry at " + id + ". Use 'list' to see valid IDs.\n");
            return null;
        }

        private static void TagCommand(string[] args)
        {
            const string usage = "Usage: tag <id> <tag>\n";
            if (args.Length != 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryIndexById(args, usage);
            if (nullableIndex is not int index)
                return;

            Entry entry = entries[index];

            string tag = args[1].ToLowerInvariant();
            if (!entry.Tags.Contains(tag))
            {
                entry.Tags.Add(tag);
                Console.WriteLine("Entry " + entry.Id + " (" + entry.Name + ") added Tag: " + tag + "\n");
            }
            else
                Console.WriteLine("Tag: " + tag + " already exists" + " on entry " + entry.Id + ".\n");
        }

        private static void UntagCommand(string[] args)
        {
            const string usage = "Usage: untag <id> <tag>\n";
            if (args.Length != 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryIndexById(args, usage);
            if (nullableIndex is not int index)
                return;

            Entry entry = entries[index];
            string tag = args[1].ToLowerInvariant();

            if (entry.Tags.Remove(tag))
                Console.WriteLine("Entry " + entry.Id + " (" + entry.Name + ") removed Tag: " + tag + "\n");
            else
                Console.WriteLine("Tag: " + tag + " not found" + " on entry " + entry.Id + ".\n");
        }

        private static void TagsCommand(string[] args)
        {
            const string usage = "Usage: tags <id>\n";
            if (args.Length != 1)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = TryGetEntryIndexById(args, usage);
            if (nullableIndex is not int index)
                return;

            Entry entry = entries[index];

            string tags = string.Join(", ", entry.Tags);
            Console.WriteLine("Entry " + entry.Id + " (" + entry.Name + ")" + (entry.Tags.Count != 0 ? "\nTags: " + tags : "\nTags: (None)") + "\n");
        }

        private static void FindCommand(string[] args)
        {
            const string usage = "Usage: find <text>\n";
            if (args.Length != 1)
            {
                Console.WriteLine(usage);
                return;
            }

            string text = args[0].ToLowerInvariant();
            List<Entry> found = [];
            foreach (Entry entry in entries)
            {
                if (entry.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(entry);
                }
                else if (entry.TargetPath.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(entry);
                }
                else if (entry.Note != null && entry.Note.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(entry);
                }
            }

            Console.WriteLine(found.Count > 0 ? "Found " + found.Count + " entries matching " + text + "." : "Found 0 entries matching " + text + ".");
            for (int i = 0; i < found.Count; i++)
            {
                Console.WriteLine("Entry " + found[i].Id + " | " + found[i].Name + " | " + found[i].TargetPath + (found[i].Note != null ? " | " + found[i].Note : "") + (found[i].Tags.Count != 0 ? " | (" + found[i].Tags.Count + " tags)" : ""));
            }
            Console.WriteLine();
        }

        private static void FilterCommand(string[] args)
        {
            const string usage = "Usage: filter tag <tag>\n";
            if (args.Length != 2 || !args[0].Equals("tag", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine(usage);
                return;
            }

            string userTag = args[1].ToLowerInvariant();
            List<Entry> found = [];
            foreach (Entry entry in entries)
            {
                if (entry.Tags.Contains(userTag))
                {
                    found.Add(entry);
                }
            }

            Console.WriteLine(found.Count > 0 ? "Found " + found.Count + " entries matching tag " + userTag + "." : "Found 0 entries matching tag " + userTag + ".");
            for (int i = 0; i < found.Count; i++)
            {
                Console.WriteLine("Entry " + found[i].Id + " | " + found[i].Name + " | " + found[i].TargetPath + (found[i].Note != null ? " | " + found[i].Note : "") + (found[i].Tags.Count != 0 ? " | (" + found[i].Tags.Count + " tags)" : ""));
            }
            Console.WriteLine();
        }
    }
}