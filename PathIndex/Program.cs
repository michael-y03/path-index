using PathIndex.Application;
using PathIndex.Application.Commands;
using PathIndex.Application.Parsing;

namespace PathIndex
{
    class Program
    {
        private static readonly AppState appState = new();

        static void Main()
        {
            Console.WriteLine("PathIndex — index and annotate local folders");
            Console.WriteLine("Type 'help' to see commands.\n");
            LoadCommand.Execute(appState);

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

                string[] tokens = Tokenizer.TokenizeInput(input);
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
                            SaveCommand.Execute(appState);
                            break;
                        case "load":
                            LoadCommand.Execute(appState);
                            break;
                        case "help":
                            HelpCommand.Execute();
                            break;
                        case "add":
                            AddCommand.Execute(commandArgs, appState);
                            break;
                        case "remove":
                            RemoveCommand.Execute(commandArgs, appState);
                            break;
                        case "edit":
                            EditCommand.Execute(commandArgs, appState);
                            break;
                        case "tag":
                            TagCommand.Execute(commandArgs, appState);
                            break;
                        case "untag":
                            UntagCommand.Execute(commandArgs, appState);
                            break;
                        case "tags":
                            TagsCommand.Execute(commandArgs, appState);
                            break;
                        case "find":
                            FindCommand.Execute(commandArgs, appState);
                            break;
                        case "filter":
                            FilterCommand.Execute(commandArgs, appState);
                            break;
                        case "info":
                            InfoCommand.Execute(commandArgs, appState);
                            break;
                        case "list":
                            ListCommand.Execute(appState);
                            break;
                        default:
                            Console.WriteLine("Unknown command: '" + command + "'. Type 'help' to see available commands.\n");
                            break;
                    }
                }
            }
        }
    }
}