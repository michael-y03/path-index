
namespace PathIndex
{
    internal class Entry
    {

        public Entry(string targetPath, string name, string? note)
        {
            TargetPath = targetPath;
            Name = name;
            Note = note;
            Tags = new List<string>();
        }

        public string TargetPath { get; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public List<String> Tags { get; set; }
    }
}
