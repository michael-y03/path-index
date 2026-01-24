
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
                string input = "";
                while (input.Length == 0)
                {
                    Console.Write("> ");
                    input = Console.ReadLine().Trim();
                }
                
                string[] tokens = TokenizeInput(input);

                string command = tokens[0].ToLower();
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
            String[] tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
           
            return tokens;
        }

        static void HelpCommand()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  help                       Show this help");
            Console.WriteLine("  add <path> [name] [note]   Add an entry (spaces aren't supported in arguments yet)");
            Console.WriteLine("  list                       List entries");
            Console.WriteLine("  info <index>               Show full details for one entry");
            Console.WriteLine("  quit / exit                Close PathIndex\n");
        }
        
        static void AddCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: add <path> [name] [note]\nNote: spaces aren’t supported in arguments yet.\n");
                return;
            }
            Entry e = new Entry(args);
            entries.Add(e);
            Console.WriteLine("Added: " + e.Name + "\n");
        }

        static void ListCommand()
        {
            for (int i=0; i < entries.Count; i++)
            {
                Console.WriteLine(i+1 + " | " + entries[i].Name + " | " + entries[i].Path + (entries[i].Note != null ? " | " + entries[i].Note : ""));
            }
            Console.WriteLine();
        }

        private static void InfoCommand(string[] args)
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No entries yet. Add one with: add <path>.\n");
                return;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: info <index>\nIndex must be a number between 1 and " + entries.Count + ".\n");
                return;
            }

            int index;
            try
            {
                index = Convert.ToInt32(args[0]);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid index: '" + args[0] + "'.\nIndex must be a number (1-" + (entries.Count) + ").\n");
                return;
            }

            if (index < 1 || index > entries.Count)
            {
                Console.WriteLine("Index out of range. Valid range: 1–"+ entries.Count + ".\nNo entry at " + index + ". Use 'list' to see valid indices.\n");
                return;
            }

            Console.WriteLine("Entry " + index + "\nName: " + entries[index - 1].Name + "\nPath: " + entries[index - 1].Path + (entries[index - 1].Note != null ? "\nNote: " + entries[index - 1].Note : "") + "\n");
        }
    }
}