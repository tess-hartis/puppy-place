using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace PuppyPlace.Domain.ValueObjects.DogValueObjects;

public record DogName
{
    public readonly string Value;

    private DogName(string value)
    {
        Value = value;
    }

    public static Validation<Error, DogName> Create(string value)
    {
        var trimmed = value.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmed))
            return Fail<Error, DogName>("Name cannot be empty");

        if (trimmed.Length < 2)
            return Fail<Error, DogName>("Name is too short");

        if (trimmed.Length > 100)
            return Fail<Error, DogName>("Name is too long");
        
        return Success<Error, DogName>(new DogName(trimmed));
    }
}