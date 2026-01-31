namespace PathIndex.Infrastructure.Persistence
{
    internal class SaveFileDto(int lastIssuedId, List<EntryDto> entries)
    {
        public int Version { get; init; } = 1;
        public int LastIssuedId { get; init; } = lastIssuedId;
        public List<EntryDto> Entries { get; init; } = entries;
    }
}
