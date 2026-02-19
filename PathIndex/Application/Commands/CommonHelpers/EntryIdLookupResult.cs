
namespace PathIndex.Application.Commands.CommonHelpers
{
    internal class EntryIdLookupResult
    {
        private readonly bool _success;
        private readonly int? _index;
        private readonly IReadOnlyList<string> _lines;

        private EntryIdLookupResult(bool success, int? index, IEnumerable<string> lines)
        {
            _success = success;
            _index = index;
            _lines = [.. lines];
        }

        public bool Success => _success;
        public int? Index => _index;
        public IReadOnlyList<string> Lines => _lines;

        public static EntryIdLookupResult Found(int index)
        {
            return new EntryIdLookupResult(true, index, []);
        }

        public static EntryIdLookupResult Fail(IEnumerable<string> lines)
        {
            return new EntryIdLookupResult(false, null, lines);
        }
    }
}