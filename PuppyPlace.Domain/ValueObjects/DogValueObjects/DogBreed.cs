using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;


namespace PuppyPlace.Domain.ValueObjects.DogValueObjects;

public record DogBreed
{
    public readonly string Value;

    private DogBreed(string value)
    {
        Value = value;
    }

    public static Validation<Error, DogBreed> Create(string value)
    {
        var trimmed = value.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmed))
            return Fail<Error, DogBreed>("Breed cannot be empty");

        if (trimmed.Length < 2)
            return Fail<Error, DogBreed>("Breed is too short");

        if (trimmed.Length > 100)
            return Fail<Error, DogBreed>("Breed is too long");
        
        
        return Success<Error, DogBreed>(new DogBreed(trimmed));
    }
}