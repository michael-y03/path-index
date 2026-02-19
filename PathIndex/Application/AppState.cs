using PathIndex.Domain;

namespace PathIndex.Application
{
    internal class AppState
    {
        private readonly List<Entry> _entries = [];
        public IReadOnlyList<Entry> Entries => _entries.AsReadOnly();
        public int LastIssuedId { get; private set; } = 0;

        public int IssueNextId()
        {
            LastIssuedId += 1;
            return LastIssuedId;
        }

        public void AddEntry(Entry e)
        {
            _entries.Add(e);
        }

        public bool RemoveEntryById(int id)
        {
            int index = _entries.FindIndex(e => e.Id == id);
            bool elementRemoved = false;
            if (index >= 0)
            {
                _entries.RemoveAt(index);
                elementRemoved = true;
            }
            return elementRemoved;
        }

        public void ReplaceState(int lastIssuedId, IEnumerable<Entry> entries)
        {
            IEnumerable<Entry> entriesCopy = [.. entries];
            _entries.Clear();
            LastIssuedId = Math.Max(entriesCopy.Any() ? entriesCopy.Max(e => e.Id) : 0, lastIssuedId);
            _entries.AddRange(entriesCopy);
        }
    }
}
