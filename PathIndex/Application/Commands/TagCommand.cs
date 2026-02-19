using PathIndex.Application.Commands.CommandHelpers;
using PathIndex.Application.Commands.CommonHelpers;
using PathIndex.Domain;

namespace PathIndex.Application.Commands
{
    internal static class TagCommand
    {
        public static CommandResult Execute(string[] args, AppState appState)
        {
            const string usage = "Usage: tag <id> <tag>";
            if (args.Length != 2)
            {
                return new CommandResult(false, [usage]);
            }

            EntryIdLookupResult result = EntryIdHelpers.TryGetEntryIndexById(args, [usage], appState);
            if (result.Index is not int index)
                return new CommandResult(false, result.Lines);

            Entry entry = appState.Entries[index];

            string tag = args[1].ToLowerInvariant();
            if (!entry.Tags.Contains(tag))
            {
                entry.AddTag(tag);
                return new CommandResult(true, ["Entry " + entry.Id + " (" + entry.Name + ") added tag: " + tag]);
            }
            else
                return new CommandResult(true, ["Tag: " + tag + " already exists" + " on entry " + entry.Id]);
        }
    }
}
