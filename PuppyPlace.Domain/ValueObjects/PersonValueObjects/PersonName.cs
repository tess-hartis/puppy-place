using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace PuppyPlace.Domain.ValueObjects.PersonValueObjects;

public record PersonName
{
    public readonly string Value;

    private PersonName(string value)
    {
        Value = value;
    }

    public static Validation<Error, PersonName> Create(string value)
    {
        var trimmed = value.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmed))
            return Fail<Error, PersonName>("Name cannot be empty");

        if (trimmed.Length < 5 && trimmed.Length > 0)
            return Fail<Error, PersonName>("Name is too short");

        if (trimmed.Length > 100)
            return Fail<Error, PersonName>("Name is too long");

        var pattern = @"^[a-zA-Z0-9\s]+$";
        
        if (!Regex.IsMatch(trimmed, pattern))
            return Fail<Error, PersonName>("Name contains invalid characters");
        
        
        return Success<Error, PersonName>(new PersonName(trimmed));
    }
}