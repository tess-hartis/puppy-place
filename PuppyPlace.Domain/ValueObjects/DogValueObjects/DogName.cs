using System.Text.RegularExpressions;
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

        if (trimmed.Length < 5 && trimmed.Length > 0)
            return Fail<Error, DogName>("Name is too short");

        if (trimmed.Length > 100)
            return Fail<Error, DogName>("Name is too long");

        var pattern = @"^[a-zA-Z0-9\s]+$";
        
        if (!Regex.IsMatch(trimmed, pattern))
            return Fail<Error, DogName>("Name contains invalid characters");
        
        
        return Success<Error, DogName>(new DogName(trimmed));
    }
}