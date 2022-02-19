using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace PuppyPlace.Domain.ValueObjects.DogValueObjects;

public record DogAge
{
    private readonly int Value;

    private DogAge(int value)
    {
        Value = value;
    }

    public static Validation<Error, DogAge> Create(int value)
    {
        if (value > 25)
            return Fail<Error, DogAge>("Too old");
        if (value < 0)
            return Fail<Error, DogAge>("Too young");

        return Success<Error, DogAge>(new DogAge(value));
    }
}