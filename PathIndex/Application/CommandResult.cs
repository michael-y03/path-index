
namespace PathIndex.Application
{
    internal class CommandResult(bool success, IEnumerable<string> lines, bool shouldExit = false)
    {
        public bool Success { get; } = success;
        public IReadOnlyList<string> Lines { get; } = (lines.Any() ? lines.ToArray() : [] );
        public bool ShouldExit { get; } = shouldExit;
    }

}
