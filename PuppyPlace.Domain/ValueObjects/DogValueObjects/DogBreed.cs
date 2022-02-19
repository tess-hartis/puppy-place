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

        if (trimmed.Length < 5 && trimmed.Length > 0)
            return Fail<Error, DogBreed>("Breed is too short");

        if (trimmed.Length > 100)
            return Fail<Error, DogBreed>("Breed is too long");

        var pattern = @"^[a-zA-Z0-9\s]+$";
        
        if (!Regex.IsMatch(trimmed, pattern))
            return Fail<Error, DogBreed>("Breed contains invalid characters");
        
        
        return Success<Error, DogBreed>(new DogBreed(trimmed));
    }
}