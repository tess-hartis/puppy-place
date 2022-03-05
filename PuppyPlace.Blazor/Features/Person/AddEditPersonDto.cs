using FluentValidation;

namespace PuppyPlace.Blazor.Features.Person;

public class AddEditPersonDto
{
    public string Name { get; set; } = "";
}

public class PersonValidator : AbstractValidator<AddEditPersonDto>
{
    public PersonValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .Length(2, 100).WithMessage("Invalid name length")
            .Must(BeValidName).WithMessage("Name cannot contain special characters");
    }

    private bool BeValidName(string name)
    {
        name = name.Replace(" ", "");
         name = name.Replace("-", "");
         return name.All(char.IsLetter);
    }
}