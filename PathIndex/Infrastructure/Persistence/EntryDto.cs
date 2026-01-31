namespace PathIndex.Infrastructure.Persistence
{
    internal class EntryDto(int id, string targetPath, string name, string? note, List<string> tags)
    {
        public int Id { get; init; } = id;
        public string TargetPath { get; init; } = targetPath;
        public string Name { get; init; } = name;
        public string? Note { get; init; } = note;
        public List<string> Tags { get; init; } = tags;
    }
}
