using FluentValidation.Results;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.Services;

public class PersonService
{
    private readonly IPersonRepository _repository;
    private readonly PersonValidator _validator;

    public PersonService(IPersonRepository repository, PersonValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }


    public async Task ValidateNewPerson(Person person)
    {
        ValidationResult results = _validator.Validate(person);

        if (results.IsValid)
        {
            await _repository.AddPersonAsync(person);
        }
        else
        {
            throw new Exception("Not valid");
        }
    }
    
}