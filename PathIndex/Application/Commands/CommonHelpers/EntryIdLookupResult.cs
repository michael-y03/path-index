
namespace PathIndex.Application.Commands.CommonHelpers
{
    internal class EntryIdLookupResult
    {
        public bool Success { get; }
        public int? Index { get; }
        public IReadOnlyList<string> Lines { get; }

        private EntryIdLookupResult(bool success, int? index, IEnumerable<string> lines)
        {
            Success = success;
            Index = index;
            Lines = lines.ToArray();
        }

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