using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects
{
    public record Name
{
    private const int MIN_LENGTH = 3;
    private const int MAX_LENGTH = 150;

    private Name(string value) => Value = value;

    public string Value { get; }

    public static Result<Name, Error> Create(string value, string invalidField)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(invalidField + ".name", "имя не может быть пустым", invalidField);
        else if (value.Length < MIN_LENGTH)
            return Error.Validation(invalidField + ".name", "слишком короткое имя", invalidField);
        else if (value.Length > MAX_LENGTH)
            return Error.Validation(invalidField + ".name", "слишком длинное имя", invalidField);

        return new Name(value);
    }
}
}