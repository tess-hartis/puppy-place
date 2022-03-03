using LanguageExt;
using LanguageExt.Common;
using MediatR;
using PuppyPlace.Domain;
using PuppyPlace.Domain.ValueObjects.DogValueObjects;
using PuppyPlace.Repository;

namespace PuppyPlace.CqrsService.DogCQ.Commands;

public class AddDogCommand : IRequest<Validation<Error, Dog>>
{
    public string Name { get; }
    public string Age { get; }
    public string Breed { get; }

    public AddDogCommand(string name, string age, string breed)
    {
        Name = name;
        Age = age;
        Breed = breed;
    }
}

public class AddDogCommandHandler : IRequestHandler<AddDogCommand, Validation<Error, Dog>>
{
    private readonly IDogRepository _dogRepository;

    public AddDogCommandHandler(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    public async Task<Validation<Error, Dog>> Handle
        (AddDogCommand command, CancellationToken cancellationToken)
    {
        var name = DogName.Create(command.Name);
        var age = DogAge.Create(command.Age);
        var breed = DogBreed.Create(command.Breed);

        var newDog = (name, age, breed)
            .Apply(Dog.Create);

        await newDog
            .Succ(async d =>
            {
                await _dogRepository.AddAsync(d);
            })
            .Fail(e => e.AsTask());

        return newDog;
    }
}