
namespace PathIndex
{
    internal class Entry
    {

        public Entry(string[] args)
        {
            Path = args[0];

            if (args.Length >= 2) 
                Name = args[1];
            else
                Name = GetFolderName(args[0]);

            if (args.Length >= 3)
                Note = args[2];
        }

        public string Path { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }

        private static string GetFolderName(string path)
        {
            if (path.EndsWith('\\'))
                path = path.Substring(0, path.Length - 1);

            string name = "";
            for (int i = path.Length-1; i >= 0; i--)
            {
                if (path[i] == '\\')
                    break;
                name += path[i];
            }

            char[] charArray = name.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
