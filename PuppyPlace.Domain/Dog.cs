using LanguageExt;
using PuppyPlace.Domain.ValueObjects.DogValueObjects;

namespace PuppyPlace.Domain;

public class Dog
{
    private Dog()
    {
        
    }    
    
    public Guid Id { get; set; }
    public DogName Name { get; set; }
    public DogAge Age { get; set; }
    public DogBreed Breed { get; set; }
    
    private List<Person> _owners = new List<Person>();
    public IEnumerable<Person> Owners => _owners;
    
    public static Dog Create(DogName name, DogAge age, DogBreed breed)
    {
        var dog = new Dog()
        {
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