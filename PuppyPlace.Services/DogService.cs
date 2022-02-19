// using System.ComponentModel.DataAnnotations;
// using FluentValidation;
// using PuppyPlace.Domain;
// using PuppyPlace.Repository;
// using ValidationResult = FluentValidation.Results.ValidationResult;
//
// namespace PuppyPlace.Services;
//
// public class DogService
// {
//     private readonly IDogRepository _repository;
//     private readonly DogValidator _validator;
//
//     public DogService(IDogRepository repository, DogValidator validator)
//     {
//         _repository = repository;
//         _validator = validator;
//     }
//
//     public async Task ValidateDog(Dog dog)
//     {
//         ValidationResult results = _validator.Validate(dog);
//
//         if (results.IsValid)
//         {
//             await _repository.AddDogAsync(dog);
//         }
//         else
//         {
//             throw new Exception("Not valid");
//         }
//     }
//
//     public async Task ValidateUpdatedDog(Dog dog)
//     {
//         ValidationResult results = _validator.Validate(dog);
//
//         if (results.IsValid)
//         {
//             await _repository.UpdateNameAsync(dog);
//         }
//         else
//         {
//             throw new Exception("Not valid");
//         }
//     }
// }