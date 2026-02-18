using PathIndex.Application;
using PathIndex.Application.Commands;
using PathIndex.Application.Parsing;

namespace PathIndex.Cli
{
    class Program
    {
        private static readonly AppState appState = new();

        static void Main()
        {
            Console.WriteLine("PathIndex — index and annotate local folders");
            Console.WriteLine("Type 'help' to see commands.\n");
            ConsoleRenderer.Render(LoadCommand.Execute([], appState));

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

                string[]? tokens = Tokenizer.TokenizeInput(input);
                if (tokens is null)
                    ConsoleRenderer.Render(new CommandResult(false, ["Quoted Argument Error."]));
                else
                {
                    CommandResult result = CommandDispatcher.Dispatch(tokens, appState);
                    ConsoleRenderer.Render(result);
                    if (result.ShouldExit)
                        break;
                }
            }
        }
    }
}