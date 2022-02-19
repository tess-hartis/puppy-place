using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using MediatR;
using PuppyPlace.Domain;
using PuppyPlace.Domain.ValueObjects.DogValueObjects;
using PuppyPlace.Repository;
using Unit = LanguageExt.Unit;


namespace PuppyPlace.CqrsService.DogCQ.Commands;

public class UpdateDogCommand : IRequest<Option<Validation<Error,Unit >>>
{
    public Guid Id { get; set; }
    public string Name { get; }
    public int Age { get; }
    public string Breed { get; }

    public UpdateDogCommand(Guid id, string name, int age, string breed)
    {
        Id = id;
        Name = name;
        Age = age;
        Breed = breed;
    }
}
public class UpdateDogCommandHandler : 
    IRequestHandler<UpdateDogCommand, Option<Validation<Error, Unit>>>
{
    private readonly IDogRepository _dogRepository;

    public UpdateDogCommandHandler(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    public async Task<Option<Validation<Error, Unit>>> Handle
        (UpdateDogCommand command, CancellationToken cancellationToken)
    {
        var dog = await _dogRepository.FindAsync(command.Id);
        var name = DogName.Create(command.Name);
        var age = DogAge.Create(command.Age);
        var breed = DogBreed.Create(command.Breed);

        var updatedDog = dog
            .Map(d => (name, age, breed)
                .Apply(d.Update));

        ignore(updatedDog
            .Map(d =>
                d.Map(async d => await _dogRepository.SaveAsync())));

        return updatedDog;

    }

}