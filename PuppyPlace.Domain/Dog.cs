using System.ComponentModel.DataAnnotations;

namespace PuppyPlace.Domain;

public class Dog
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Breed { get; set; }
    public List<Person> Owners { get; set; }
    
    public Dog(string name, int age, string breed)
    {
        Name = name;
        Age = age;
        Breed = breed;
        Owners = new List<Person>();
    }
    public string Bark()
    {
        return "WOOF!!!";
    }
}