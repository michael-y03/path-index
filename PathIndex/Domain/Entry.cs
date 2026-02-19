namespace PathIndex.Domain
{
    public class Entry
    {
        readonly private int _id;
        readonly private string _targetPath;
        private string _name;
        private string? _note;
        private List<string> _tags;

        public Entry(int id, string targetPath, string name, string? note, IEnumerable<string>? tags = null)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be negative");
            _id = id;
            if (targetPath is null)
                throw new ArgumentNullException(nameof(targetPath), "TargetPath must not be null");
            _targetPath = targetPath;
            if (name is null)
                throw new ArgumentNullException(nameof(name), "Name must not be null");
            _name = name;
            _note = note;
            _tags = (tags == null ? [] : [.. tags]);
        }

        public int Id => _id;
        public string TargetPath => _targetPath;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string? Note
        {
            get => _note;
            set => _note = value;
        }
        public IReadOnlyList<string> Tags
        {
            get => _tags.AsReadOnly();
        }

        public void AddTag(string tag) { _tags.Add(tag); }
        public bool RemoveTag(string tag) { return _tags.Remove(tag); }
    }
}
