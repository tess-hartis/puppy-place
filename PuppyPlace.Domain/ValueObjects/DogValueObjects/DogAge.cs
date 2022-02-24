using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace PuppyPlace.Domain.ValueObjects.DogValueObjects;

public record DogAge
{
    public readonly string Value;

    private DogAge(string value)
    {
        Value = value;
    }

    public static Validation<Error, DogAge> Create(string value)
    {
        if (!int.TryParse(value, out var parsed)) 
            return Fail<Error, DogAge>("Invalid age");
        
        if (parsed > 25 || parsed < 0)
        {
            return Fail<Error, DogAge>("Invalid age");
        }

        return Success<Error, DogAge>(new DogAge(value));

    }
}