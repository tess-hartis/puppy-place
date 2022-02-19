using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using LanguageExt;
using PuppyPlace.Domain.ValueObjects.PersonValueObjects;

namespace PuppyPlace.Domain;

public class Person
{
    public Guid Id { get; set; }
    public PersonName Name { get; set; }
    
    private List<Dog> _dogs = new List<Dog>();
    public IEnumerable<Dog> Dogs => _dogs;
    
    public Person(PersonName name)
    {
        Name = name;
        Id = Guid.NewGuid();
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