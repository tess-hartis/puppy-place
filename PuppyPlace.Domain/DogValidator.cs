// using System.Data;
// using FluentValidation;
//
// namespace PuppyPlace.Domain;
//
// public class DogValidator : AbstractValidator<Dog>
// {
//     public DogValidator()
//     {
//         RuleFor(d => d.Name)
//             .Cascade(CascadeMode.Stop)
//             .NotEmpty().WithMessage("cannot be empty")
//             .Length(2, 20).WithMessage("must be between 2 and 20 characters")
//             .Must(BeValidName).WithMessage("Cannot contain special characters");
//         RuleFor(d => d.Breed)
//             .Cascade(CascadeMode.Stop)
//             .NotEmpty().WithMessage("cannot be empty")
//             .Length(2, 20).WithMessage("must be between 2 and 20 characters")
//             .Must(BeValidName).WithMessage("Cannot contain special characters");
//         RuleFor(d => d.Age)
//             .Cascade(CascadeMode.Stop)
//             .NotEmpty().WithMessage("Age cannot be empty")
//             .Must(BeValidAge).WithMessage("Age must be between 0 and 25");
//     }
//     
//     private bool BeValidName(string name)
//     {
//         name = name.Replace(" ", "");
//         name = name.Replace("-", "");
//         return name.All(Char.IsLetter);
//     }
//
//     private bool BeValidAge(int age)
//     {
//         if (age >= 0 && age <= 25)
//         {
//             return true;
//         }
//         
//         return false;
//     }
//     
// }
