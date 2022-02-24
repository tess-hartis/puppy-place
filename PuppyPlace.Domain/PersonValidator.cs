// using FluentValidation;
//
// namespace PuppyPlace.Domain;
//
// public class PersonValidator : AbstractValidator<Person>
// {
//     public PersonValidator()
//     {
//         RuleFor(p => p.Name)
//             .Cascade(CascadeMode.Stop)
//             .NotEmpty().WithMessage("cannot be empty")
//             .Length(2, 20).WithMessage("must be between 2 and 20 characters")
//             .Must(BeValidName).WithMessage("Cannot contain special characters");
//
//     }
//
//  private bool BeValidName(string name)
//  {
//      name = name.Replace(" ", "");
//      name = name.Replace("-", "");
//      return name.All(Char.IsLetter);
//  }
// }