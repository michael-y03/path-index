using PathIndex.Application;

namespace PathIndex.Cli
{
    internal static class ConsoleRenderer
    {
        public static void Render(CommandResult result)
        {
            foreach (string line in result.Lines)
            {
                if (result.Success)
                    Console.WriteLine(line);
                else
                    Console.Error.WriteLine(line);
            }
        }
    }
}
