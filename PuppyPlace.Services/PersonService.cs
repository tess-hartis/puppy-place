using FluentValidation.Results;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.Services;

public class PersonService
{
    private readonly PersonRepository _repository;
    private readonly PersonValidator _validator;

    public PersonService(PersonRepository repository, PersonValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task Validate(Person person)
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