
namespace PathIndex.Application
{
    public class CommandResult
    {
        private readonly bool _success;
        private readonly IReadOnlyList<string> _lines;
        private readonly bool _shouldExit;

        public CommandResult(bool success, IEnumerable<string> lines, bool shouldExit = false)
        {
            _success = success;
            if (lines is null)
                throw new ArgumentNullException(nameof(lines), "CommandResult must always include lines");
            _lines = lines.ToArray();
            _shouldExit = shouldExit;
        }

        public bool Success => _success;
        public IReadOnlyList<string> Lines => _lines;
        public bool ShouldExit => _shouldExit;
    }
}
