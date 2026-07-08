using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects
{
    public record Slug
    {
        private const int MIN_LENGTH = 3;
        private const int MAX_LENGTH = 150;

        private Slug(string value) => Value = value;

        public string Value { get; }

        public static Result<Slug, Error> Create(string value, string invalidField)
        {
            bool isLetter = true;

            foreach (var symbol in value)
            {
                isLetter = Char.IsLetter(symbol);

                if (!isLetter)
                    return Error.Validation(invalidField + ".Identifier",
                        "идентификатор должен состоять только из латиницы", invalidField);
            }

            if (string.IsNullOrWhiteSpace(value))
                return Error.Validation(invalidField + ".Identifier", "идентификатор не может быть пустым", invalidField);
            else if (value.Length < MIN_LENGTH)
                return Error.Validation(invalidField + ".Identifier", "слишком короткий идентификатор", invalidField);
            else if (value.Length > MAX_LENGTH)
                return Error.Validation(invalidField + ".Identifier", "слишком длинный идентификатор", invalidField);

            return new Slug(value);
        }
    }
}