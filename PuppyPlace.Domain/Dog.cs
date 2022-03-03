using LanguageExt;
using PuppyPlace.Domain.ValueObjects.DogValueObjects;

namespace PuppyPlace.Domain;

public class Dog
{
    private Dog() { }    
    
    public Guid Id { get; private init; }
    public DogName Name { get; private set; }
    public DogAge Age { get; private set; }
    public DogBreed Breed { get; private set; }
    
    private readonly List<Person> _owners = new List<Person>();
    public IEnumerable<Person> Owners => _owners;
    
    public static Dog Create(DogName name, DogAge age, DogBreed breed)
    {
        var dog = new Dog()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Age = age,
            Breed = breed,
        };

        return dog;
    }
    
    public Unit AddOwner(Person person)
    {
        _owners.Add(person);
        return Unit.Default;
    }

    public Unit Update(DogName name, DogAge age, DogBreed breed)
    {
        Name = name;
        Age = age;
        Breed = breed;
        return Unit.Default;
    }
}