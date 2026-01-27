
namespace PathIndex
{
    internal class Entry(int id, string targetPath, string name, string? note)
    {
        public int Id { get; } = id;
        public string TargetPath { get; } = targetPath;
        public string Name { get; set; } = name;
        public string? Note { get; set; } = note;
        public List<String> Tags { get; set; } = new List<string>();
    }
}
