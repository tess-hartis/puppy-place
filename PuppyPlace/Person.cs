namespace PuppyPlace;

public class Person
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public List<Dog> Dogs { get; set; }

    public Person(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        Dogs = new List<Dog>();
    }

    public void ShowPersonDogs()
    { 
        Console.WriteLine($"{Name} has the following dogs:");
            
        foreach (var dog in Dogs)
        {
            Console.WriteLine($"{dog.Name}");
        }
    }

    public void ShowPersonId()
    {
        Console.WriteLine($"{Name} has the following ID:");
        Console.WriteLine(Id);
    }

    public void ChangeName(string updatedName)
    {
        Name = updatedName;
    }
    
    
}