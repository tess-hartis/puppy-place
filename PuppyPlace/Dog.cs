namespace PuppyPlace;

public class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Breed { get; set; }
    public List<Person> Owner { get; set; }

    public Dog(string name, int age, string breed)
    {
        Name = name;
        Age = age;
        Breed = breed;
        Owner = new List<Person>();
    }

    public string Bark()
    {
        return "WOOF!!!";
    }

    public void AddDogOwner(Person owner)
    {
        Owner.Add(owner);
    }

    public void ShowOwners()
    {
        Console.WriteLine($"{Name} belongs to these people:");
        foreach (var owner in Owner)
        {
            Console.WriteLine($"{owner.Name}");
        }
    }

}