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

        if (trimmed.Length < 2)
            return Fail<Error, PersonName>("Name is too short");

        if (trimmed.Length > 100)
            return Fail<Error, PersonName>("Name is too long");

        if(!BeValidName(trimmed))
            return Fail<Error, PersonName>("Name cannot contain special characters");
        
        return Success<Error, PersonName>(new PersonName(trimmed));
    }
    
    private static bool BeValidName(string name)
    {
        name = name.Replace(" ", "");
        name = name.Replace("-", "");
        return name.All(char.IsLetter);
    }
}