using FluentValidation;

namespace PuppyPlace.Domain;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(p => p.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("cannot be empty")
            .Length(2, 20).WithMessage("must be between 2 and 20 characters");
    }
}