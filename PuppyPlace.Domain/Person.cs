using System.ComponentModel.DataAnnotations;

namespace PuppyPlace.Domain;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Dog> Dogs { get; set; }

    public Person(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        Dogs = new List<Dog>();
    }

    public void ShowPersonDogs()
    { 
        System.Console.WriteLine($"{Name} has the following dogs:");
            
        foreach (var dog in Dogs)
        {
            System.Console.WriteLine($"{dog.Name}");
        }
    }

    public void ShowPersonId()
    {
        System.Console.WriteLine($"{Name} has the following ID:");
        System.Console.WriteLine(Id);
    }

    public void ChangeName(string updatedName)
    {
        Name = updatedName;
    }
    
    
}