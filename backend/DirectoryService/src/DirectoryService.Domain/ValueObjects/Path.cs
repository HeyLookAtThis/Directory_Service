namespace DirectoryService.Domain.ValueObjects
{
    public record Path
    {
        private const char SEPARATOR = '.';

        public Path(Path? parentPath, string identifier)
        {
            if (parentPath == null)
                Value = identifier;
            else
                Value = parentPath.Value + SEPARATOR + identifier;
        }

        public static char Separator => SEPARATOR;

        public string Value { get; }
    }
}