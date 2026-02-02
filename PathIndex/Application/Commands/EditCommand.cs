
using PathIndex.Domain;
using PathIndex.Application.Commands.CommandHelpers;

namespace PathIndex.Application.Commands
{
    internal static class EditCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: edit <id> name <value>\n       edit <id> note <value>\n       edit <id> clear-note\n";
            if (args.Length < 2)
            {
                Console.WriteLine(usage);
                return;
            }

            int? nullableIndex = EntryIdHelpers.TryGetEntryIndexById(args, usage, appState);
            if (nullableIndex is not int index)
                return;
            Entry entry = appState.Entries[index];
            string field = args[1].ToLowerInvariant();
            if (field != "clear-note" && field != "name" && field != "note")
            {
                Console.WriteLine("Invalid field: '" + field + "'.\nValid fields: name, note, clear-note.\n" + usage);
                return;
            }

            if (field == "clear-note")
            {
                if (args.Length != 2)
                {
                    Console.WriteLine(usage);
                    return;
                }
                ClearNote(entry);
                return;
            }
            if (args.Length != 3)
            {
                Console.WriteLine(usage);
                return;
            }
            if (field == "name")
            {
                UpdateName(entry, args[2]);
                return;
            }
            if (field == "note")
            {
                UpdateNote(entry, args[2]);
                return;
            }
        }

        private static void UpdateName(Entry entry, string newName)
        {
            string originalName = entry.Name;
            entry.Name = newName;
            Console.WriteLine("Updated name: " + originalName + " to " + newName + "\n");
        }

        private static void UpdateNote(Entry entry, string newNote)
        {
            string originalNote = "(" + (entry.Note ?? "none") + ")";
            entry.Note = newNote;
            Console.WriteLine("Updated note: " + originalNote + " -> " + newNote + "\n");
        }

        private static void ClearNote(Entry entry)
        {
            string? originalNote = entry.Note;
            entry.Note = null;
            Console.WriteLine(originalNote == null ? "Nothing to clear.\n" : "Cleared note: (" + originalNote + ")\n");
        }
    }
}
