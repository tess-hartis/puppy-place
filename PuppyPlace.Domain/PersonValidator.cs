using FluentValidation;

namespace PuppyPlace.Domain;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}