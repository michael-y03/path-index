using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Application.Commands.CommonHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class TagsCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: tags <id>";
            if (args.Length != 1)
            {
                return new CommandResult(false, [usage]);
            }

            EntryIdLookupResult result = EntryIdHelpers.TryGetEntryIndexById(args, [usage], appState);
            if (result.Index is not int index)
                return new CommandResult(false, result.Lines);

            Entry entry = appState.Entries[index];

            string tags = string.Join(", ", entry.Tags);
            return new CommandResult(true, ["Entry " + entry.Id + " (" + entry.Name + ")" + (entry.Tags.Count != 0 ? " Tags: " + tags : " Tags: (none)")]);
        }
    }
}
