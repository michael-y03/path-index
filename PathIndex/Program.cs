
namespace PathIndex
{
    class Program
    {
        static List<Entry> entries = new List<Entry>();

        static void Main(string[] args)
        {
            Console.WriteLine("PathIndex — local path index + tagging/notes");
            Console.WriteLine("C#/.NET learning project.\n");

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
                    case "help":
                        HelpCommand();
                        break;
                    case "add":
                        AddCommand(commandArgs);
                        break;
                    case "list":
                        ListCommand();
                        break;
                    default:
                        Console.WriteLine("Unknown Command.\n");
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
            Console.WriteLine("  help                       Show this help text");
            Console.WriteLine("  add <path> [name] [note]   Add a new entry (No spaces in path/name/note yet.)");
            Console.WriteLine("  list                       List all entries added this session");
            Console.WriteLine("  quit                       Close the program\n");
        }
        
        static void AddCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Incorrect Usage: add <path>\n");
                return;
            }
            entries.Add(new Entry(args));
        }

        static void ListCommand()
        {
            for (int i=0; i < entries.Count; i++)
            {
                Console.WriteLine(i + " | " + entries[i].Name + " | " + entries[i].Path + (entries[i].Note != null ? " | " + entries[i].Note : ""));
            }
            Console.WriteLine();
        }
    }
}