
using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Application.Commands.CommonHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class EditCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            List<string> usage = ["Usage: edit <id> name <value>", "edit <id> note <value>", "edit <id> clear-note"];
            if (args.Length < 2)
            {
                return new CommandResult(false, usage);
            }

            EntryIdLookupResult result = EntryIdHelpers.TryGetEntryIndexById(args, usage, appState);
            if (result.Index is not int index)
                return new CommandResult(false, result.Lines);
            Entry entry = appState.Entries[index];
            string field = args[1].ToLowerInvariant();
            if (field != "clear-note" && field != "name" && field != "note")
            {
                return new CommandResult(false, ["Invalid field: '" + field + "'.Valid fields: name, note, clear-note." + usage]);
            }
            string message;
            if (field == "clear-note")
            {
                if (args.Length != 2)
                {
                    return new CommandResult(false, usage);
                }
                message = ClearNote(entry);
                return new CommandResult(true, [message]);
            }
            if (args.Length != 3)
            {
                return new CommandResult(false, usage);
            }
            if (field == "name")
            {
                message = UpdateName(entry, args[2]);
                return new CommandResult(true, [message]);
            }
            message = UpdateNote(entry, args[2]);
            return new CommandResult(true, [message]);
        }

        private static string UpdateName(Entry entry, string newName)
        {
            string originalName = entry.Name;
            entry.Name = newName;
            return ("Updated name: " + originalName + " to " + newName);
        }

        private static string UpdateNote(Entry entry, string newNote)
        {
            string originalNote = "(" + (entry.Note ?? "none") + ")";
            entry.Note = newNote;
            return ("Updated note: " + originalNote + " -> " + newNote);
        }

        private static string ClearNote(Entry entry)
        {
            string? originalNote = entry.Note;
            entry.Note = null;
            return (originalNote == null ? "Nothing to clear." : "Cleared note: (" + originalNote + ")");
        }
    }
}
