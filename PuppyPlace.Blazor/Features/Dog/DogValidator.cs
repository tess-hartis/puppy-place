using FluentValidation;

namespace PuppyPlace.Blazor.Features.Dog;

public class DogValidator : AbstractValidator<AddEditDogDto>
{
    public DogValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .Length(2, 100).WithMessage("Invalid name length")
            .Must(BeValidName).WithMessage("Name cannot contain special characters");;
        RuleFor(x => x.Age)
            .NotEmpty().WithMessage("Age cannot be empty")
            .Must(BeValidInt).WithMessage("Invalid age");
        RuleFor(x => x.Breed)
            .NotEmpty().WithMessage("Breed cannot be empty")
            .Length(2, 100).WithMessage("Invalid breed length")
            .Must(BeValidName).WithMessage("Breed cannot contain special characters");;
    }
    
    private bool BeValidName(string name)
    {
        name = name.Replace(" ", "");
        name = name.Replace("-", "");
        return name.All(char.IsLetter);
    }

    private bool BeValidInt(string number)
    {
        if (!int.TryParse(number, out var parsed)) 
            return false;
        
        if (parsed > 25 || parsed < 0)
        {
            return false;
        }

        return true;

    }
}