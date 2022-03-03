using LanguageExt;
using PuppyPlace.Domain.ValueObjects.PersonValueObjects;

namespace PuppyPlace.Domain;

public class Person
{
    private Person() { }
    
    public Guid Id { get; private init; }
    public PersonName Name { get; private set; }
    
    private readonly List<Dog> _dogs = new List<Dog>();
    public IEnumerable<Dog> Dogs => _dogs;
    
    public static Person Create(PersonName name)
    {
        var person = new Person()
        {
            Name = name,
            Id = Guid.NewGuid()
        };

        return person;
    }

    public Unit Update(PersonName name)
    {
        Name = name;
        return Unit.Default;
    }

    public Unit AdoptDog(Dog dog)
    {
        _dogs.Add(dog);
        return Unit.Default;
    }
}